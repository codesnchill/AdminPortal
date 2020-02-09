// validate if end date > start date
$("head").append('<script type="text/javascript" src="' + 'https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.18.1/moment.min.js' + '"></script>');
function dateTimeIsValid(startDate, startTime, endDate, endTime) {

    isValid = true;
    errorMsg = "";

    startDateTime = startDate + " " + startTime;

    endDateTime = endDate + " " + endTime;

    if (startDate !== "" && startTime !== "" && endDate !== "" && endTime !== "") {
        if (moment(startDateTime).isAfter(endDateTime)) {
            errorMsg += "start datetime should be earlier than end datetime";
            isValid = false;
        }
    }

    //check if startdatetime is later than enddatetime

    return { errorMsg: errorMsg, isValid: isValid };
}

function dateTimeIsNotEmpty(input) {

    isValid = true;
    errorMsg = "";

    if (input === "") {

        errorMsg += "Please fill out the required fields"
        isValid = false;
    }
    return { errorMsg: errorMsg, isValid: isValid };
}

//check if select2 dropdown is empty
function departmentDropdownIsValid(input) {
    isValid = true;
    errorMsg = "";

    if (input == null) {
        isValid = false;
        errorMsg = "Please select an option";
    }
    return { errorMsg: errorMsg, isValid: isValid };
}

// check if input has no illegal characters (json format only) and not empty
// pass in $(element).val here
function txtInputIsValid(input) {

    isValid = true;

    //regex for illegal characters
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/]+/;
    errorMsg = "";

    // if input contains only whitespace OR input is empty (string only)
    if (/^\s+$/.test(input) || input === ""){
        errorMsg += "Please fill out the required fields";
        isValid = false;
    }

    // if input contains illegal characters
    if (format.test(input)) {
        errorMsg += "Input can't contain special characters"
        isValid = false;
    }

    return { errorMsg: errorMsg, isValid: isValid };
}
function passwordValidation(input) {
    isValid = true;
    errorMsg = "";

    //regex for / \ ' "
    var format = /^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{8,}$/;

    //Check input Contain(/ \ ' ")
    
    if (!format.test(input)) {
        errorMsg += "Password must Contain at least 1 Uppercase,1 lowercase,1 number and 1 special character and Password length must contain minimum 8 characters.";
        isValid = false;
    }
    return { errorMsg: errorMsg, isValid: isValid };

}