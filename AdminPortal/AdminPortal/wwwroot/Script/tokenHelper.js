function TokenWebFormData(inCollectedRefreshToken) {
    this.refresh_token = inCollectedRefreshToken;
}
// check if token in session has not expired yet
function tokenIsValid() {
    // get session
    $sessionHandler = $.ajax({
        type: 'GET',
        url: '/Account/getTokenSession',
        dataType: 'json',
        contentType: 'application/json;',
    })//end of jQuery.ajax() call
    $sessionHandler.done(function (data, textStatus, jqXHR) {
        //expiration is in UTC
        var sessionObject = JSON.parse(data);
        var expirationDate = sessionObject.Token_expiration;
        var refresh_token = sessionObject.Refresh_token;
        console.log(expirationDate)
        // refersh if session is expired
        if (expirationDate < Date.now() / 1000) {
            refreshTokenSession(refresh_token)
        }
        // token is valid once token is refreshed (if expired)
        return true;
    });

    cosole.log("FAILED TO GET SESSION");
}


// ************TODO: Change refreshtoken method to take the session item from account controller instead.
// refresh token stored in session
function refreshTokenSession(inRefresh_token) {

    // start of ajax call refresh token
    var tokenWebFormData = new TokenWebFormData(inRefresh_token);
    var tokenWebFormDataInString = JSON.stringify(tokenWebFormData);
    console.log(tokenWebFormDataInString);

    var url = "https://localhost:44300/api/v1/refreshtoken"

    $tokenHandler = jQuery.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        contentType: 'application/json;',
        data: "'" + tokenWebFormDataInString + "'"
    })//end of jQuery.ajax() call
    $tokenHandler.done(function (data, textStatus, jqXHR) {

        var tokenSessionObject = {
            Token1: data.token.toString(),
            Refresh_token: inRefresh_token.toString(),
            Token_expiration: data.token_expiration.toString() //Timezone is in UTC
        }

        sessionStorage.setItem("tokenSessionObject", JSON.stringify(tokenSessionObject));

        // set new token in session
        var sessionObject = JSON.parse(sessionStorage.getItem('tokenSessionObject'));
        var expirationDate = sessionObject.token_expiration;
        var token = sessionObject.token;

        console.log("Successfully refreshed token");
        //store token in session
        console.log("Token: " + token);
        console.log("Expiration(UTC): " + expirationDate);
        console.log("Refresh token: " + sessionObject.refresh_token)
    });//end of $tokenHandler.done()


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

function getTokenSession() {
    
}