window.onload = function () {
    //alert("JS connected");

    
    var formHandle = document.forms.TeacherForm;

    formHandle.onsubmit = function () {

        //alert("Form submitted");
        var teacherFname = formHandle.FirstName;
        var teacherLname = formHandle.LastName;
        var employeeNum = formHandle.EmployeeNumber;
        var hireDate = formHandle.HireDate;
        var salary = formHandle.Salary;
        //validate each input element
        if (teacherFname.value === "" || teacherFname.value === null) {
            teacherFname.style.background = "#f5d0d0";
            teacherFname.focus();
            return false;
        } else if (teacherLname.value === "" || teacherLname.value === null) {
            teacherLname.style.background = "#f5d0d0";
            teacherLname.focus();
            return false;
            //check that the employee number does not exceed three digits
        } else if (employeeNum.value > 999 || employeeNum.value < 100 || employeeNum === null) {
            employeeNum.style.background = "#f5d0d0"
            employeeNum.focus();
            return false;
        } else if (hireDate.value === "") {
            hireDate.style.background = "#f5d0d0";
            hireDate.focus();
            return false;
            //check that the proposed salary is within an acceptable range
        } else if (salary.value < 0 || salary.value > 100 || salary.value === "") {
            salary.style.background = "#f5d0d0";
            salary.focus();
            return false;
        }
    }
}