var typeActivity = "";

function setActivityId(e, id) {
    var x = document.getElementById("editActivityId");
    x.value = id;

    var parent = e.parentElement;
    var columns = parent.cells;
    var type = "Normal";
    if (columns.length == 6) type = "All";
    var body = document.getElementById("editActivityBody");
    body.innerHTML = "";

    for (i = 0; i < columns.length - 1; i++) {
        var header = tableHeadersDictionairy[type][i];
        console.log(i, header);
        console.log(columns[i], columns[i].innerHTML);
        
        var group = document.createElement("div");
        group.classList += ["form-group"];

        var label = document.createElement("label");
        label.innerHTML = header;

        var input = document.createElement("input");
        input.classList += ["form-control"];
        input.required = true;
        input.name = header;
        input.value = columns[i].innerHTML;
        input.id = `EditModal_${header}`

        if (header == "Date") {
            input.type = "date";
            var date = columns[i].innerHTML.split(" ")[0].split("/");
            input.value = `${date[2]}-${date[1]}-${date[0]}`;

        } else if (header == "Duration") {
            input.type = "time";
            input.step = 1;
            input.addEventListener("change", calculateCalories)
        } else if (header == "Calories" || header == "Distance") {
            input.type = "number";
            input.min = 1;
            input.step = 0.01;
        }

        if (header == "Calories" || header == "Type") {
            input.readOnly = "readonly"
        }

        group.appendChild(label);
        group.appendChild(input);

        body.append(group);
        //console.log(header);
        
        console.log(body);
    }


    var x = document.getElementById("deleteActivityId");
    x.value = id;
    //console.log(e)
}


function calculateCalories() {
    var typeActivity = document.getElementById("EditModal_Type").value
    console.log(typeActivity)

    var time = document.getElementById("EditModal_Duration").value.split(":")
    var hours = (time[0] == "NaN" || time[0] == undefined) ? "0" : time[0];
    var minutes = (time[1] == "NaN" || time[1] == undefined) ? "0" : time[1];
    var seconds = (time[2] == "NaN" || time[2] == undefined) ? "0" : time[2];
    console.log(hours, minutes, seconds)
    var timeInMinutes = (parseInt(hours) * 60) + parseInt(minutes) + (parseInt(seconds) / 60)


    if (typeActivity == "Cycling") MET = 8;
    else if (typeActivity == "Running") MET = 9;
    else if (typeActivity == "Hiking") MET = 3;
    else MET = 6;
    console.log(typeActivity, timeInMinutes)
    var calories = ((MET * 65 * 3.5) / 200) * timeInMinutes
    document.getElementById("EditModal_Calories").value = Math.round(calories)
    console.log(typeActivity, timeInMinutes, calories)
}