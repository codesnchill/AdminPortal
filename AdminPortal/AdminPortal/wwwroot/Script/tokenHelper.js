﻿function TokenWebFormData(inCollectedRefreshToken) {
    this.refresh_token = inCollectedRefreshToken;
}
// check if token in session has not expired yet
//function tokenIsValid() {
//    // get session
//    $sessionHandler = $.ajax({
//        type: 'GET',
//        url: '/Account/getTokenSession',
//        dataType: 'json',
//        contentType: 'application/json;',
//    })//end of jQuery.ajax() call
//    $sessionHandler.done(function (data, textStatus, jqXHR) {
//        //expiration is in UTC
//        var sessionObject = JSON.parse(data);
//        var expirationDate = sessionObject.Token_expiration;
//        var refresh_token = sessionObject.Refresh_token;
//        console.log(refresh_token)
//        console.log("Date.parse(expirationDate).now() / 1000 = " + Date.parse(expirationDate.toString()).now() / 1000);
//        console.log("Date.now() / 1000 = " + Date.now() / 1000);
//        if (Date.parse(expirationDate).now() / 1000 < Date.now() / 1000) {
//            refreshTokenSession(refresh_token);
//            console.log("NOW REFRESHING");
//        }
//        // token is valid once token is refreshed (if expired)
//        return true;
//    });
//    $sessionHandler.fail(function (data, textStatus, jqXHR) {
//        //if if fail, empty out the session
//    });
//}

function tokenIsValid(cb) {
    // get session
    //var valid;
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/tokenIsValid',
        dataType: 'json',
        contentType: 'application/json;',
        //success: function (result) {
        //    cb()
        //}
    })//end of jQuery.ajax() call
    //return valid;
}

function refreshTokenSession(inRefresh_token) {

    $sessionHandler = $.ajax({
        type: 'POST',
        url: '/Account/refreshToken',
        data: "'{Refresh_token : " + inRefresh_token + "}'",
        contentType: 'application/json;'
        //data: "'" + JSON.stringify(stringifiedTokenObj) + "'"
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {

        var sessionObject = JSON.parse(data);
        var token = sessionObject["token"];
        var token_expiration = sessionObject["token_expiration"];
        console.log(sessionObject);
        console.log(token);
        console.log(token_expiration);
        var tokenSessionObject = {
            Token1: token,
            Refresh_token: inRefresh_token,
            Token_expiration: token_expiration //Timezone is in UTC
        };

        // after getting new token, set it to session
        setTokenSession(tokenSessionObject);

        return true;
    });
}

function setTokenSession(stringifiedTokenObj) {
    console.log(stringifiedTokenObj)
    $sessionHandler = $.ajax({
        type: 'POST',
        url: '/Account/setTokenSession',
        data: JSON.stringify(stringifiedTokenObj),
        contentType: 'application/json;'
        //data: "'" + JSON.stringify(stringifiedTokenObj) + "'"
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {

        return true;
    });
}

function setEmployeeSession(stringifiedEmployeeObj) {
    console.log(stringifiedEmployeeObj)
    $sessionHandler = $.ajax({
        type: 'POST',
        url: '/Account/setEmployeeSession',
        data: JSON.stringify(stringifiedEmployeeObj),
        contentType: 'application/json;',
        //data: "'" + JSON.stringify(stringifiedEmployeeObj) + "'"
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {

        return true;
    });
}

function getTokenSession(cb) {
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/getTokenSession',
        dataType: 'json',
        contentType: 'application/json;',
        success: function (data) {
            var sessionObject = JSON.parse(data);
            if (data == null) {
                return false;
            }
            var expirationDate = sessionObject.Token_expiration;
            var refresh_token = sessionObject.Refresh_token;
            var token = sessionObject.Token1;
            console.log(expirationDate);
            console.log(refresh_token);
            console.log(token);

            cb(sessionObject)
            // token is valid once token is refreshed (if expired)
        }
    })//end of jQuery.ajax() call

    $sessionHandler.fail(function (data, textStatus, jqXHR) {
        console.log("FAILED TO GET TOKEN SESSION");
    });
}

function getEmployeeSession() {
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/getEmployeeSession',
        dataType: 'json',
        contentType: 'application/json;',
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {
        //expiration is in UTC
        var sessionObject = JSON.parse(data);
        console.log(sessionObject);

        if (data == null) {
            console.log("empty session data");
            return false;
        }
        // token is valid once token is refreshed (if expired)
        return sessionObject;
    });
}

function clearSession() {
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/clearSession',
        dataType: 'json',
        contentType: 'application/json;',
        success: function (result) {
            console.log("CLEARED SESSION");
        }
    })//end of jQuery.ajax() call
    
}
