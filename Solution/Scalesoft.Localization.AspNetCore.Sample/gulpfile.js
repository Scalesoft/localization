/// <binding Clean="clean"/>

const gulp = require("gulp"),
    concat = require("gulp-concat"),
    cleanCSS = require("gulp-clean-css"),
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
    path = require("path"),
    webpack = require('webpack'),
    webpackConfig = require('./webpack.config.js');
    
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
    // compileTypescript: "compile:typescript",
    webpackTypescript: "webpack:ts",
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
    linkExternalProject: "link-external-project",
    linkToExternalProject: "link-to-external-project",
    bootstrapExternalProject: "bootstrap-external-project",
    linkToExternalProjectProduction: "link-external-project-production",
};

let enableSwallowTsError = false;

function swallowTsError(error) {
    process.stderr.write(`${error.toString()}\n`);

    this.emit("end");
}

const getBundles = regexPattern => bundleconfig.filter(
    bundle => regexPattern.test(bundle.outputFileName),
);

gulp.task(taskNames.buildExternalProject,
    (callback) => {
        var yarn = (process.platform === "win32" ? "yarn.cmd" : "yarn");
        const externalBuild = spawn(yarn, ["build"], { cwd: paths.localizationWebScriptProject });

        externalBuild.on("close", (code) => {
            callback(code);
        });

        externalBuild.on("error", (err) => {
            console.error(err);
        });
    },
);

gulp.task(taskNames.linkExternalProject,
    (callback) => {
        var yarn = (process.platform === "win32" ? "yarn.cmd" : "yarn");
        const externalGulp = spawn(yarn, ["link"], { cwd: paths.localizationWebScriptProject });

        externalGulp.on("close", (code) => {
            callback(code);
        });

        externalGulp.on("error", (err) => {
            console.error(err);
        });
    },
);

gulp.task(taskNames.linkToExternalProject,
    (callback) => {
        linkDependency(callback);
    },
);

function linkDependency(callback, cwd) {
    const yarn = (process.platform === "win32" ? "yarn.cmd" : "yarn");
    const externalGulp = spawn(yarn, ["link", "scalesoft-localization-web"], {cwd});

    externalGulp.on("close", (code) => {
        callback(code);
    });

    externalGulp.on("error", (err) => {
        console.error(err);
    });
}

gulp.task(taskNames.linkToExternalProjectProduction,
    (callback) => {
        linkDependency(callback, paths.webroot);
    },
);

gulp.task(taskNames.bootstrapExternalProject,
    gulp.series(
        taskNames.buildExternalProject,
        taskNames.linkExternalProject,
        taskNames.linkToExternalProject
    ));

gulp.task(taskNames.lintTs,
    () => tsProject.src()
        .pipe(tslint({
            formatter: "verbose"
        }))
        .pipe(tslint.report()),
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
        ),
);

gulp.task(taskNames.lintSassFixer,
    () => gulp.src(paths.sass)
        .pipe(stylelint({
            failAfterError: true,
            reporters: [
                {formatter: "verbose", console: true},
            ],
            fix: true,
        })),
);

/*
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
    },
);
*/

function getWebpackErrorMessages(errors) {
    let errorMessages = [];
    for (let i = 0; i < errors.length; i++) {
        errorMessages.push(errors[i].message);
    }
    return errorMessages;
}

gulp.task(taskNames.webpackTypescript,
    (resolve, reject) => {
        return webpack(webpackConfig, (err, stats) => {
            if (err) {
                return reject(err)
            }
            if (stats.hasErrors()) {
                const messages = getWebpackErrorMessages(stats.compilation.errors);
                return reject(new Error(messages.join('\n')))
            }
            resolve()
        });
    });

gulp.task(taskNames.compileSass,
    () => gulp.src(paths.sass)
        .pipe(sourcemaps.init())
        .pipe(sass().on("error", sass.logError))
        .pipe(sourcemaps.write("."))
        .pipe(gulp.dest(paths.css)),
);

gulp.task(taskNames.sassWatch,
    () => gulp.watch(paths.sass, gulp.parallel(taskNames.bundleAndMinifyCss)),
);

gulp.task(taskNames.typescriptWatch,
    () => {
        enableSwallowTsError = true;

        return gulp.watch(paths.typescript, gulp.parallel(taskNames.bundleAndMinifyJs));
    },
);

gulp.task(taskNames.packageWatch,
    () => gulp.watch(paths.packageJson, gulp.parallel(taskNames.downloadProductionDeps)),
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
        },
);

gulp.task(taskNames.bundleAndMinifyJs,
    gulp.series(
        gulp.parallel(
            taskNames.lintTs,
            taskNames.webpackTypescript
        ),
        taskNames.bundleJs
    ),
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
                    .pipe(sourcemaps.init())
                    .pipe(cleanCSS())
                    .pipe(sourcemaps.write());
            }

            return gulpStream.pipe(gulp.dest("."));
        },
    );

    bundleCssTasksName[bundleConfigKey] = taskName;
}

gulp.task(taskNames.bundleCss,
    bundleCssTasksName.length > 0
        ? gulp.parallel(...bundleCssTasksName)
        : done => {
            done();
        },
);

gulp.task(taskNames.bundleAndMinifyCss,
    gulp.series(
        gulp.parallel(
            taskNames.compileSass
            //TODO enable when possible
            //taskNames.lintSass
        ),
        taskNames.bundleCss
    ),
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
    },
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
        // taskNames.linkToExternalProjectProduction, // Link is not required, because sample is using JavaScript import keyword
        taskNames.deleteProductionPackageJson
    ),
);

gulp.task(taskNames.cleanJs,
    () => del([paths.js]),
);

gulp.task(taskNames.cleanCss,
    () => del([paths.css]),
);

gulp.task(taskNames.cleanDeps,
    () => del([paths.runtimedeps]),
);

gulp.task(taskNames.clean,
    gulp.parallel(taskNames.cleanJs, taskNames.cleanCss, taskNames.cleanDeps),
);

if (fs.existsSync("../skip-gulp-run")) {
    gulp.task("default", (done) => done());
}
else {
    gulp.task("default",
        gulp.series(
            // taskNames.bootstrapExternalProject, // Disable external project build, use csproj invoked build instead
            taskNames.downloadAllDeps,
            taskNames.bundleAndMinify
        ),
    );
}

gulp.task(taskNames.watch,
    gulp.parallel(taskNames.sassWatch, taskNames.typescriptWatch, taskNames.packageWatch),
);
