function TokenWebFormData(inCollectedRefreshToken) {
    this.refresh_token = inCollectedRefreshToken;
}

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

            cb(sessionObject)
            // token is valid once token is refreshed (if expired)
        }
    })//end of jQuery.ajax() call

    $sessionHandler.fail(function (data, textStatus, jqXHR) {
        console.log("FAILED TO GET TOKEN SESSION");
    });
}

function getEmployeeSession(cb) {
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/getEmployeeSession',
        dataType: 'json',
        contentType: 'application/json;'
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {
        //expiration is in UTC
        var sessionObject = JSON.parse(data);

        if (data == null) {
            return false;
        }
        // token is valid once token is refreshed (if expired)
        cb(sessionObject);
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
