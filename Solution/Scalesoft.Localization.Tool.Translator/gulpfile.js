/// <binding BeforeBuild='yarn-runtime, default' ProjectOpened='yarn-runtime' />

var gulp = require("gulp"),
    sourcemaps = require("gulp-sourcemaps"),
    ts = require("gulp-typescript"),
    yarn = require("gulp-yarn");
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


//Download yarn dependencies

gulp.task("yarn-runtime", function () {
    return gulp.src(["./wwwroot/package.json", "./wwwroot/yarn.lock"])
        .pipe(yarn());
});

// Main build

gulp.task("default", gulp.parallel("build:ts"));
