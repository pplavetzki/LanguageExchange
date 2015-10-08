/**
 * Created by paul on 5/22/15.
 */
module.exports = function() {
    "use strict";
    var build = './client/build/';
    var codeRoot = './client/public/';
    var codeApp = codeRoot + 'app/';
    var clientApp = build + 'app/';

    var config = {
        build: build,
        clientApp: clientApp,
        rootFile: 'index.html',
        codeRoot: codeRoot,
        codeApp: codeApp,
        codeAppFile: codeApp + 'index.js',
        codeAppHtml: codeApp + 'index.html',
        codeCleanHtml: codeApp + 'index-clean.html',
        viewsDest: clientApp + 'views',
        assetsDest: build + 'assets',
        vendorDest: build + 'vendor',
        imagesDest: clientApp + 'images',
        dataDest: clientApp + 'data',
        cssDest: clientApp + 'stylesheets',
        cacheKeys: [
            'users',
            'groups',
            'systems',
            'labs',
            'devices'
        ],
        nodeServer: './server/index.js',
        defaultPort: 4343,
        server:'./server/',
        alljs: [
            './*.js',
            './client/public/app/**/*.js'
        ]
    };

    return config;
};