/**
 * Created by paul on 5/6/15.
 */
var express = require('express');

var app = express();

app.use(express.static('./client/build/app'));
app.use(express.static('./client/build'));

app.all('/*', function(req, res) {
    res.sendFile('index.html', { root: 'client/build/app' });
});

app.listen(4343, function () {

    console.log('Language Exchange app rocking!');

});