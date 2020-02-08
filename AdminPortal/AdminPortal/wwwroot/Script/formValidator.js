// validate if end date > start date
$("head").append('<script type="text/javascript" src="' + 'https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.18.1/moment.min.js' + '"></script>');
function dateTimeIsValid(startDate, startTime, endDate, endTime) {

    isValid = true;
    errorMsg = "";

    if (startDate === "" || startTime === "" || endDate === "" || endTime === "") {
        errorMsg += "Please fill out the required fields"
        isValid = false;

        //return the moment any of these inputs are empty
        return { errorMsg: errorMsg, isValid: isValid };
    }

    startDateTime = startDate + " " + startTime;

    endDateTime = endDate + " " + endTime;

    if (moment(startDateTime).isAfter(endDateTime)) {
        errorMsg += "start datetime should be earlier than end datetime";
        isValid = false;
    }

    //check if startdatetime is later than enddatetime

    return { errorMsg: errorMsg, isValid: isValid };
}

//check if select2 dropdown is empty
function departmentDropdownIsValid(input) {
    isValid = true;
    errorMsg = "";

    if (input == null) {
        isValid = false;
        errorMsg = "Please select the department";
    }
    return { errorMsg: errorMsg, isValid: isValid };
}

// check if input has no illegal characters (json format only) and not empty
// pass in $(element).val here
function txtInputIsValid(input) {

    isValid = true;

    //regex for illegal characters
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/;
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
