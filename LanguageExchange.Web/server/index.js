/**
 * Created by paul on 5/6/15.
 */
var express = require('express');
var jwt = require('jwt-simple');
var moment = require('moment');
var http = require('http');
var bodyParser = require('body-parser');
var querystring = require('querystring');

var app = express();

app.use(bodyParser.json());       // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({
     // to support URL-encoded bodies
    extended: true
})); 

var redis = require("redis"),
    client = redis.createClient({host:'10.211.55.65',port:'6379'});

app.use(express.static('./client/build/app'));
app.use(express.static('./client/build'));

client.on("connect", function (err) {
    console.log("connected!");
    //getData();
});

function getData() {
    client.get('refresh:pplavetzki', function (err, reply) {
        console.log(reply.toString());
    });
}
var secret = '6T9+akYtzZVwOztXmkso/Jrg0FvuEp0KdtZvQ+bFeLc=';
var clientId = 'LangExApp';

function login(data, callback) {
    var postData = querystring.stringify({
        grant_type: "password",
        password : data.password,
        userName : data.userName,
        client_id : clientId,
        client_secret : secret
    });
    
    var response = '';
    
    var options = {
        protocol: 'http:',
        hostname: 'localhost',
        port: 49425,
        path: '/token',
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
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

function requestToken(callback) {
    
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
            'Content-Type': 'application/x-www-form-urlencoded',
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

app.post('/auth/login', function (req, res, next) {
    login(req.body, function (response) {
        var obj = JSON.parse(response);
        res.json({ access_token: obj.access_token });
    });
});

app.all('/*', function(req, res) {
    res.sendFile('index.html', { root: 'client/build/app' });
});

app.listen(4343, function () {

    console.log('Language Exchange app rocking!');

});