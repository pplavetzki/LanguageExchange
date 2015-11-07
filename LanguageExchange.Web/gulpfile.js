/**
 * Created by paul on 5/26/15.
 */
/**
 * Created by paul on 5/7/15.
 */
var gulp = require('gulp');
var browserify = require('gulp-browserify');
var concat = require('gulp-concat');

var plumber = require('gulp-plumber');

var uglify = require('gulp-uglify');
var config = require('./gulp.config')();
var argv = require('yargs').argv;
var gulpif = require('gulp-if');
var nodemon = require('gulp-nodemon');

var browserSync = require('browser-sync').create();
var reload      = browserSync.reload;

var util = require('gulp-util');
var minifyCss = require('gulp-minify-css');
var minifyHTML = require('gulp-minify-html');
var jshint = require('gulp-jshint');

var sftp = require('gulp-sftp');

var debug = argv.debug || true;
var port = config.defaultPort;

var ugly = function(){
    "use strict";
    if(debug && debug === 'true'){
        return false;
    }
    else{
        return true;
    }
};

gulp.task('vet', function(){
    "use strict";
    return gulp.src(config.alljs)
        .pipe(plumber())
        .pipe(jshint())
        .pipe(jshint.reporter('jshint-stylish', {verbose: true}))
        .pipe(jshint.reporter('fail'));
});

// Browserify task
gulp.task('browserify', function () {
    'use strict';
    return gulp.src([config.codeAppFile])
        .pipe(plumber())
        .pipe(browserify({
            insertGlobals: true,
            debug: debug
        }))
        // Bundle to a single file
        .pipe(concat('bundle.js'))
        /*jshint camelcase: false */
        //.pipe(gulpif(ugly(), uglify({compress:{drop_console:true}})))
        // Output it to our dist folder
        .pipe(gulp.dest(config.clientApp));
});

gulp.task('views1', [], function () {
    "use strict";
    return gulp.src(config.codeAppHtml)
        // And put it in the dist folder
        //.pipe(minifyHTML())
        .pipe(gulp.dest(config.clientApp));
});

gulp.task('views2', [], function(){
    "use strict";
    return gulp.src(['!./client/public/app/*', './client/public/app/**/*.html'])
        //.pipe(minifyHTML())
        .pipe(gulp.dest(config.viewsDest));
});

gulp.task('views', ['views1', 'views2'], function(){});

gulp.task('asset1', [], function(){
    "use strict";
    return gulp.src(['./client/public/assets/**/*'])
        .pipe(gulp.dest(config.assetsDest));
});

gulp.task('asset2', [], function(){
    "use strict";
    return gulp.src(['./client/public/vendor/**/*'])
        .pipe(gulp.dest(config.vendorDest));
});

gulp.task('images', [], function(){
    "use strict";
    return gulp.src(['./client/public/app/images/*'])
        .pipe(gulp.dest(config.imagesDest));
});

gulp.task('styles', function(){
    "use strict";
    return gulp.src(['./client/public/app/stylesheets/*'])
        .pipe(minifyCss())
        .pipe(gulp.dest(config.cssDest));
});

gulp.task('seed-data', [], function(){
    "use strict";
    return gulp.src(['./client/public/app/data/*'])
        .pipe(gulp.dest(config.dataDest));
});

function getNodeOptions(){
    "use strict";
    return {
        script: config.nodeServer,
        delayTime: 1,
        env: {
            'PORT': port
        },
        watch: [config.server]
    };
}

function startBrowserSync(isDev, specRunner) {
    'use strict';
    if (browserSync.active) {
        return;
    }

    log('Starting BrowserSync on port: ' + port);

    var options = {
        proxy: 'localhost:' + port,
        port: 9095,
        ghostMode: { // these are the defaults t,f,t,t
            clicks: true,
            location: false,
            forms: true,
            scroll: true
        },
        injectChanges: true,
        logFileChanges: true,
        logLevel: 'debug',
        logPrefix: 'web-boilerplate',
        //browser: 'google chrome',
        notify: true,
        reloadOnRestart: true,
        reloadDelay: 1000
    };

    browserSync.init(options);
}

function log(msg) {
    'use strict';
    if (typeof(msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                util.log(util.colors.blue(msg[item]));
            }
        }
    } else {
        util.log(util.colors.blue(msg));
    }
}

function serve() {
    'use strict';
    log('starting to serve');

    var nodeOptions = getNodeOptions();

    nodemon(nodeOptions)
        .on('restart', [], function(ev) {

            log('*** nodemon restarted');
            log('files changed:\n' + ev);
            setTimeout(function() {
                browserSync.notify('reloading now ...');
                reload({stream:false});
            }, 0);
        })
        .on('start', function () {
            log('*** nodemon started');
            startBrowserSync(true, false);
        })
        .on('crash', function () {
            log('*** nodemon crashed: script crashed for some reason');
        })
        .on('exit', function () {
            log('*** nodemon exited cleanly');
        });
}

gulp.task('assets', ['asset1', 'asset2'], function(){});

gulp.task('static', ['images', 'views', 'styles', 'seed-data'], function(){});

gulp.task('process', ['browserify', 'static'], function(){});

gulp.task('build', ['vet', 'process'], function(){});

gulp.task('serve', ['process'], function(){
    "use strict";
    serve();

    gulp.watch(['client/public/app/*.js', 'client/public/app/**/*.js'], ['browserify', reload]);
    gulp.watch(['client/public/app/*.html', 'client/public/app/**/*.html'], ['views', reload]);
    gulp.watch(['client/public/app/stylesheets/*.css'], ['styles', reload]);
});