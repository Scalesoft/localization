const gulp = require("gulp");
const ts = require("gulp-typescript");
const sourcemaps = require("gulp-sourcemaps");
const uglify = require('gulp-uglify');
const rename = require('gulp-rename');

const tsProject = () => {
	const tsProjectPipe = ts.createProject("tsconfig.json");

	return tsProjectPipe.src()
		.pipe(sourcemaps.init())
		.pipe(tsProjectPipe());
};

gulp.task("build:ts", () =>
	tsProject().js
		.pipe(sourcemaps.write('.'))
		.pipe(gulp.dest("dist"))
);

gulp.task("build:dts", () =>
	tsProject().dts.pipe(gulp.dest("dist"))
);

gulp.task("minify:js", () =>
	gulp.src(["dist/*.js", "!dist/*.min.js"])
		.pipe(uglify())
		.pipe(rename({extname: '.min.js'}))
		.pipe(gulp.dest('./dist'))
);


gulp.task("default", gulp.parallel([
	gulp.series([
		"build:ts",
		"minify:js",
	]),
	"build:dts",
]));
