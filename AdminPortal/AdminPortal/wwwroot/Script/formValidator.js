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

function emailIsValid(email) {

    isValid = true;
    errorMsg = "";

    if (/^\s+$/.test(email) || email === "") {
        errorMsg = "Please fill out the required fields"
        isValid = false;
        return { errorMsg: errorMsg, isValid: isValid };
    }

    //if email is not valid
    if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))) {
        errorMsg = "Please enter a valid email address";
        isValid = false;
        return { errorMsg: errorMsg, isValid: isValid };
    }

    return { errorMsg: errorMsg, isValid: isValid };
}

function employeeIdIsValid(input) {

    isValid = true;
    errorMsg = "";

    // check if input is not ONLY digits
    // and if input is not 8 chars lng
    if (input.length != 8) {
        errorMsg = "Please enter a valid Employee ID";
        isValid = false;
        return { errorMsg: errorMsg, isValid: isValid };
    }

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
