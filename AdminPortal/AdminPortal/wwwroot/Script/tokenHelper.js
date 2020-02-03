
function TokenWebFormData(inCollectedRefreshToken) {
    this.refresh_token = inCollectedRefreshToken;
}
// check if token in session has not expired yet
function tokenIsValid() {
    //expiration is in UTC
    var sessionObject = JSON.parse(sessionStorage.getItem('tokenSessionObject'));
    var expirationDate = sessionObject.token_expiration;
    var refresh_token = sessionObject.refresh_token;
    console.log(expirationDate)
    // refersh if session is expired
    if (expirationDate < Date.now() / 1000) {
        refreshTokenSession(refresh_token)
    }

    // token is valid once token is refreshed (if expired)
    return true;
}

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
            token: data.token.toString(),
            refresh_token: inRefresh_token,
            token_expiration: data.token_expiration.toString() //Timezone is in UTC
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

