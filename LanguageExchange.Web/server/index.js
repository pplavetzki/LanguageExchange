/**
 * Created by paul on 5/6/15.
 */
var express = require('express');
var jwt = require('jwt-simple');

var secret = '6T9+akYtzZVwOztXmkso/Jrg0FvuEp0KdtZvQ+bFeLc=';
var issuer = 'http://pareidoliasw';
var audience = 'http://localhost:10100';

var payload = {}

var app = express();

app.use(express.static('./client/build/app'));
app.use(express.static('./client/build'));

app.get('/auth/scope', function (req, res) {
    res.json({ token: '9vjkw239z++' });
});

app.all('/*', function(req, res) {
    res.sendFile('index.html', { root: 'client/build/app' });
});

app.listen(4343, function () {

    console.log('Language Exchange app rocking!');

});