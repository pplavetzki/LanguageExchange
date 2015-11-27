/**
 * Created by paul on 5/6/15.
 */
var express = require('express');
var jwt = require('jwt-simple');
var moment = require('moment');
var http = require('http');

var app = express();

app.use(express.static('./client/build/app'));
app.use(express.static('./client/build'));

function requestToken(callback) {
    var secret = '6T9+akYtzZVwOztXmkso/Jrg0FvuEp0KdtZvQ+bFeLc=';
    var scope = 'WebBridge';
    var issuer = 'PareidoliaSW';
    var aud = 'http://localhost:10100';
    var secret64 = new Buffer(secret, 'base64');
    
    var iat = parseInt(moment.utc().valueOf() / 1000);
    var exp = parseInt(moment.utc().add(1, 'h').valueOf() / 1000);
    
    var payload = { aud: aud, iss: issuer, scope: scope, iat: iat, exp: exp, nbf: iat };
    var token = jwt.encode(payload, secret64);

    var postData = JSON.stringify( {
        'token' : token
    });
    
    var response = '';
    
    var options = {
        protocol: 'http:',
        hostname: 'localhost',
        port: 10100,
        path: '/authorize/token',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': postData.length
        }
    };
    
    var req = http.request(options, function (res) {
        console.log('STATUS: ' + res.statusCode);
        console.log('HEADERS: ' + JSON.stringify(res.headers));
        res.setEncoding('utf8');
        res.on('data', function (chunk) {
            response += chunk;
        });
        res.on('end', function () {
            console.log('No more data in response.')
            callback(response);
        })
    });
    
    req.on('error', function (e) {
        console.log('problem with request: ' + e.message);
    });
    
    // write data to request body
    req.write(postData);
    req.end();
}

app.get('/auth/scope', function (req, res) {
    requestToken(function (data, error) {
        res.json(JSON.parse(data));
    });
});

app.all('/*', function(req, res) {
    res.sendFile('index.html', { root: 'client/build/app' });
});

app.listen(4343, function () {

    console.log('Language Exchange app rocking!');

});