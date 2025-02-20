/// <binding BeforeBuild='npm-runtime' ProjectOpened='npm-runtime' />

var gulp = require("gulp"),
    sourcemaps = require("gulp-sourcemaps"),
    ts = require("gulp-typescript"),
    exec = require("child_process").exec;
var tsProject = ts.createProject("tsconfig.json");

var paths = {
    webroot: "./wwwroot/"
};


// TypeScript build
gulp.task("build:ts", function () {
    var tsResult = tsProject.src()
        .pipe(sourcemaps.init())
        .pipe(tsProject());

    return tsResult.js
        .pipe(sourcemaps.write(".", { sourceRoot: "." }))
        .pipe(gulp.dest(paths.webroot + "js"));
});


//Download npm dependencies

gulp.task("npm-runtime", function (cb) {
    exec("npm install", { cwd: "./wwwroot" }, function (err, stdout, stderr) {
        console.log(stdout);
        console.error(stderr);
        cb(err);
    });
});

// Main build

gulp.task("default", gulp.parallel("build:ts", "npm-runtime"));
