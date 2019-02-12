/// <binding Clean="clean"/>

const gulp = require("gulp"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-dart-sass"),
    typescript = require("gulp-typescript"),
    sourcemaps = require("gulp-sourcemaps"),
    del = require("del"),
    yarn = require("gulp-yarn"),
    fs = require("fs"),
    tslint = require("gulp-tslint"),
    stylelint = require("gulp-stylelint"),
    bundleconfig = require("./bundleconfig.json"),
    spawn = require("child_process").spawn,
    path = require("path");
    
const paths = {
    webroot: `./wwwroot/`,
    nodemodules: `./node_modules/`,
    packageJson: `./package.json`,
    yarnRc: `./.yarnrc`,
    yarnLock: `./yarn.lock`,
    sass: `./Styles/**/*.scss`,
    scripts: `./Scripts`,
    typescript: `./Scripts/**/*.ts`,
    tsconfig: `./Scripts/tsconfig.json`,
    localizationWebScriptProject: `./../Scalesoft.Localization.Web.Script/`
};

const regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

paths.css = paths.webroot + "css/";
paths.js = paths.webroot + "js/";
paths.runtimedeps = paths.webroot + "node_modules/";

const tsProject = typescript.createProject(paths.tsconfig);

const taskNames = {
    lintTs: "lint:ts",
    lintSass: "lint:sass",
    lintSassFixer: "lint:sass:fix",
    compileSass: "compile:sass",
    compileTypescript: "compile:typescript",
    sassWatch: "watch:sass",
    typescriptWatch: "watch:typescript",
    packageWatch: "watch:package",
    watch: "watch",
    cleanCss: "clean:css",
    cleanJs: "clean:js",
    cleanDeps: "clean:deps",
    cleanDevDeps: "clean:dev-deps",
    clean: "clean",
    downloadAllDeps: "download",
    downloadProductionDeps: "download:production-deps",
    deleteProductionPackageJson: "delete-production-package-json",
    bundleJs: "bundle:js",
    bundleCss: "bundle:css",
    bundleAndMinifyJs: "minify:js",
    bundleAndMinifyCss: "minify:css",
    bundleAndMinify: "minify",
    buildExternalProject: "build-external-project",
    packExternalProject: "pack-external-project",
    installExternalProject: "install-external-project",
};

let enableSwallowTsError = false;

function swallowTsError(error) {
    process.stderr.write(`${error.toString()}\n`);

    this.emit("end");
}

const getBundles = regexPattern => bundleconfig.filter(
    bundle => regexPattern.test(bundle.outputFileName)
);

gulp.task(taskNames.buildExternalProject,
    (callback) => {
        var yarn = (process.platform === "win32" ? "yarn.cmd" : "yarn");
        const externalGulp = spawn(yarn, ["gulp"], { cwd: paths.localizationWebScriptProject });

        externalGulp.on("close", (code) => {
            callback(code);
            if (code !== 0) {
                callback(code);
                return;
            }
            gulp.task(taskNames.packExternalProject)(callback);
        });

        externalGulp.on("error", (err) => {
            console.error(err);
        });
    }
);

gulp.task(taskNames.packExternalProject,
    (callback) => {
        var npm = (process.platform === "win32" ? "npm.cmd" : "npm");
        const npmPack = spawn(npm, ["pack"], { cwd: paths.localizationWebScriptProject });

        let packName;
        npmPack.stdout.on("data", (data) => {
            packName = data.toString();
        });

        npmPack.on("close", (code) => {
            if (code !== 0) {
                callback(code);
                return;
            }
            gulp.task(taskNames.installExternalProject)(callback, packName);
        });

        npmPack.on("error", (err) => {
            console.error(err);
        });
    }
);

gulp.task(taskNames.installExternalProject,
    (callback, packName) => {
        const fullpath = path.join(paths.localizationWebScriptProject, packName);
        var yarn = (process.platform === "win32" ? "yarn.cmd" : "yarn");
        const releaseGulp = spawn(yarn, ["add", "--no-lockfile", `scalesoft-localization-web@${fullpath}`]);

        releaseGulp.on("close", (code) => {
            if (code !== 0) {
                callback(code);
                return;
            }
            const devGulp = spawn(yarn, ["add", "--no-lockfile", "--dev", `@types/scalesoft-localization-web@${fullpath}`]);

            devGulp.on("close", (devCode) => {
                callback(devCode);
                return;
            });

            devGulp.on("error", (err) => {
                console.error(err);
            });
        });
        
        releaseGulp.on("error", (err) => {
            console.error(err);
        });
    }
);

gulp.task(taskNames.lintTs,
    () => tsProject.src()
        .pipe(tslint({
            formatter: "verbose"
        }))
        .pipe(tslint.report())
);

gulp.task(taskNames.lintSass,
    () => gulp.src(paths.sass)
        .pipe(
            stylelint({
                failAfterError: true,
                reporters: [
                    {formatter: "string", console: true},
                ],
            })
        )
);

gulp.task(taskNames.lintSassFixer,
    () => gulp.src(paths.sass)
        .pipe(stylelint({
            failAfterError: true,
            reporters: [
                {formatter: "verbose", console: true},
            ],
            fix: true,
        }))
);

gulp.task(taskNames.compileTypescript,
    () => {
        const tsResult = tsProject.src()
            .pipe(sourcemaps.init())
            .pipe(
                tsProject().on("error",
                    enableSwallowTsError ? swallowTsError : () => process.exit(1)
                )
            );

        return tsResult.js
            .pipe(sourcemaps.write("."))
            .pipe(gulp.dest(paths.js));
    }
);

gulp.task(taskNames.compileSass,
    () => gulp.src(paths.sass)
        .pipe(sourcemaps.init())
        .pipe(sass().on("error", sass.logError))
        .pipe(sourcemaps.write("."))
        .pipe(gulp.dest(paths.css))
);

gulp.task(taskNames.sassWatch,
    () => gulp.watch(paths.sass, gulp.parallel(taskNames.bundleAndMinifyCss))
);

gulp.task(taskNames.typescriptWatch,
    () => {
        enableSwallowTsError = true;

        return gulp.watch(paths.typescript, gulp.parallel(taskNames.bundleAndMinifyJs));
    }
);

gulp.task(taskNames.packageWatch,
    () => gulp.watch(paths.packageJson, gulp.parallel(taskNames.downloadProductionDeps))
);

const bundleJsTasksName = [];
const bundleConfigsJs = getBundles(regex.js);
for (const bundleConfigKey in bundleConfigsJs) {
    if (!bundleConfigsJs.hasOwnProperty(bundleConfigKey)) {
        continue;
    }

    const taskName = `${taskNames.bundleJs}:${bundleConfigKey}`;

    gulp.task(taskName,
        () => {
            const bundle = bundleConfigsJs[bundleConfigKey];

            const inputs = [];
            for (const input of bundle.inputFiles) {
                inputs.push(`${paths.webroot}${input}`);
            }

            let gulpStream = gulp.src(inputs, {base: "."})
                .pipe(sourcemaps.init())
                .pipe(concat(`${paths.webroot}${bundle.outputFileName}`));

            if (bundle.minify && bundle.minify.enabled) {
                gulpStream = gulpStream.pipe(uglify());
            }

            return gulpStream.pipe(sourcemaps.write("."))
                .pipe(gulp.dest("."));
        }
    );

    bundleJsTasksName[bundleConfigKey] = taskName;
}

gulp.task(taskNames.bundleJs,
    bundleJsTasksName.length > 0
        ? gulp.parallel(...bundleJsTasksName)
        : done => {
            done();
        }
);

gulp.task(taskNames.bundleAndMinifyJs,
    gulp.series(
        gulp.parallel(
            taskNames.lintTs,
            taskNames.compileTypescript
        ),
        taskNames.bundleJs
    )
);

const bundleCssTasksName = [];
const bundleConfigsCss = getBundles(regex.css);
for (const bundleConfigKey in bundleConfigsCss) {
    if (!bundleConfigsCss.hasOwnProperty(bundleConfigKey)) {
        continue;
    }

    const taskName = `${taskNames.bundleCss}:${bundleConfigKey}`;

    gulp.task(taskName,
        () => {
            const bundle = bundleConfigsCss[bundleConfigKey];

            const inputs = [];
            for (const input of bundle.inputFiles) {
                inputs.push(`${paths.webroot}${input}`);
            }

            let gulpStream = gulp.src(inputs, {base: "."})
                .pipe(concat(`${paths.webroot}${bundle.outputFileName}`));

            if (bundle.minify && bundle.minify.enabled) {
                gulpStream = gulpStream
                    .pipe(cssmin());
            }

            return gulpStream.pipe(gulp.dest("."));
        }
    );

    bundleCssTasksName[bundleConfigKey] = taskName;
}

gulp.task(taskNames.bundleCss,
    bundleCssTasksName.length > 0
        ? gulp.parallel(...bundleCssTasksName)
        : done => {
            done();
        }
);

gulp.task(taskNames.bundleAndMinifyCss,
    gulp.series(
        gulp.parallel(
            taskNames.compileSass
            //TODO enable when possible
            //taskNames.lintSass
        ),
        taskNames.bundleCss
    )
);

gulp.task(taskNames.bundleAndMinify,
    gulp.parallel(
        taskNames.bundleAndMinifyJs,
        taskNames.bundleAndMinifyCss
    )
);

gulp.task(taskNames.downloadProductionDeps,
    () => {
        if (!fs.existsSync(paths.webroot)) {
            fs.mkdirSync(paths.webroot);
        }

        return gulp.src(["./package.json", "./yarn.lock"])
            .pipe(gulp.dest(paths.webroot))
            .pipe(yarn(
                {
                    production: true,
                    noProgress: true,
                    noBinLinks: true,
                    nonInteractive: true,
                    ignoreScripts: true, // is it good idea?
                }
            ));
    }
);

gulp.task(taskNames.deleteProductionPackageJson,
    () => del([
        `${paths.webroot}/package.json`,
        `${paths.webroot}/yarn.lock`,
        `${paths.webroot}/.yarnrc`,
    ])
);

gulp.task(taskNames.downloadAllDeps,
    gulp.series(
        taskNames.downloadProductionDeps,
        taskNames.deleteProductionPackageJson
    )
);

gulp.task(taskNames.cleanJs,
    () => del([paths.js])
);

gulp.task(taskNames.cleanCss,
    () => del([paths.css])
);

gulp.task(taskNames.cleanDeps,
    () => del([paths.runtimedeps])
);

gulp.task(taskNames.clean,
    gulp.parallel(taskNames.cleanJs, taskNames.cleanCss, taskNames.cleanDeps)
);

if (fs.existsSync("../skip-gulp-run")) {
    gulp.task("default", (done) => done());
}
else {
    gulp.task("default",
        gulp.series(
            taskNames.buildExternalProject,
            taskNames.downloadAllDeps,
            taskNames.bundleAndMinify
        )
    );
}

gulp.task(taskNames.watch,
    gulp.parallel(taskNames.sassWatch, taskNames.typescriptWatch, taskNames.packageWatch)
);
