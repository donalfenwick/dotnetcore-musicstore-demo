var gulp = require('gulp');
var less = require('gulp-less');
var concat = require('gulp-concat');
var npmDist = require('gulp-npm-dist');
var rename = require('gulp-rename');
var ts = require("gulp-typescript");

var packageJson = require('./package.json');
var tsProject = ts.createProject('./tsconfig.json');

//copy all non-dev NPM libs to the wwwroot  
gulp.task("copyNpmLibs", function () {
    gulp.src(npmDist(), {base:'./node_modules/'})
        .pipe(rename(function(path) {
            path.dirname = path.dirname.replace(/\/dist/, '').replace(/\\dist/, '');
        }))
        .pipe(gulp.dest('./wwwroot/libs'));
});

gulp.task('less', function () {
    gulp.src(['./less/**/*.less'])
        .pipe(less())
        .pipe(concat('site-styles.css'))
        .on('error', function (err) {
            console.log(err.toString());
            this.emit('end');
        })
        .pipe(gulp.dest('./wwwroot/css'));
    });
    
    gulp.task('compile-typescript', function () {
        return tsProject.src()
            .pipe(tsProject())
            .js.pipe(gulp.dest('./wwwroot/js/compiled'));
    });

gulp.task('default', ['copyNpmLibs','less', 'compile-typescript', 'watch']);

gulp.task('watch', function () {
    gulp.watch('less/**/*.less', ['less']);
    gulp.watch('typescript/**/*.ts', ['compile-typescript']);
});