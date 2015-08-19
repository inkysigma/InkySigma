/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');

gulp.task('minify', function () {
    gulp.src("./wwwroot/lib/**/*.js")
        .pipe(uglify())
        .pipe(gulp.dest("./wwwroot/js"));
});