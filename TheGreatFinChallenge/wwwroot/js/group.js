function changeGroupCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("groupCards");    //Get all elements with fitnessCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}

function changeMutualFriendsCard(i, n) {
    // This function will display a certain card with information on press.
    var x = document.getElementsByClassName("mutualFriendsCard");    //Get all elements with fitnessCards as className
    var htmlCur = x[i]
    var htmlNext = x[n]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlCur.classList.add("d-none")
}

function changeData(i, n, step, groupName, activity) {
    // This function will display a certain chart with information on press.
    var x = document.getElementsByClassName("lineChartHeaders");    //Get all headers with lineChartHeaders as className
    var htmlCur = x[i + step]
    var htmlNext = x[n + step]

    // Change displaysettings between current and next card
    htmlNext.classList.remove("d-none")
    htmlNext.classList.add("d-flex")

    htmlCur.classList.remove("d-flex")
    htmlCur.classList.add("d-none")
    //console.log(x)
    //console.log(i, n)
    //console.log(window[`data${name}`])

    // Update chart data with smooth transition and dynamic data based on the n
    //console.log(step/6, activity)
    //console.log(dictOfCharts[step/7])
    myLineChart = dictOfCharts[step / 7]
    //console.log(LineChartData[groupName][activity])
    if (groupName == "Ranking") {
        myLineChart.options.legend = {
            display: true,
            position: 'bottom'
        }
        colors = [
            "rgba(128, 0, 0, 1)",
            "rgba(250, 190, 212, 1)",
            "rgba(245, 130, 49, 1)",
            "rgba(128, 128, 0, 1)",
            "rgba(225, 225, 25, 1)",
            "rgba(66, 212, 244, 1)",
            "rgba(0, 0, 117, 1)",
            "rgba(145, 30, 180, 1)"
        ]
        var keys = Object.keys(LineChartDataGroupRanking[activity])
        datasets = []
        yscale = 0
        for (i = 0; i < keys.length; i++) {
            var key = keys[i]
            var dataLst = LineChartDataGroupRanking[activity][key]
            //console.log(i, key, dataLst)

            datasets.push({
                label: key,
                data: dataLst,
                order: i + 1,
                lineTension: 0.3,
                backgroundColor: "rgba(255, 255, 255, 1)",
                borderColor: colors[i],
                pointRadius: 3,
                pointBackgroundColor: colors[i],
                pointBorderColor: colors[i],
                pointHoverRadius: 3,
                pointHoverBackgroundColor: colors[i],
                pointHoverBorderColor: colors[i],
                pointHitRadius: 10,
                pointBorderWidth: 2
            })

            num = Math.max.apply(null, dataLst) + 1
            if (num > yscale) yscale = num
        }
    } else {
        var label = "Activities"
        if (activity == "Calories") label = "Calories"
        if (activity == "Distance") label = "Distance"

        datasets = [{
            label: label,
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: LineChartData[groupName][activity]
        }]

        yscale = Math.max.apply(null, LineChartData[groupName][activity]) + 1
    }
    if (yscale > 6) yscale = 5; 
    myLineChart.data.datasets = datasets;
    myLineChart.update()
    //console.log()
}

//RESETTING DATA WITH RIGHT HEADER
function updateData(el, step, groupName, activity) {
    var x = document.getElementsByClassName("lineChartHeaders");
    for (i = 0; i < x.length; i++) {
        x[i].classList.remove("d-none")
        x[i].classList.remove("d-flex")
        x[i].classList.add("d-none")
    }

    var htmlCur = x[step]
    //console.log(htmlCur)
    htmlCur.classList.remove("d-none")
    htmlCur.classList.add("d-flex")

    //console.log(step, step/7, activity)
    //console.log(dictOfCharts[step/7])
    myLineChart = dictOfCharts[step / 7]

    //console.log(LineChartData[groupName])
    //console.log(LineChartData[groupName][activity])

    if (groupName == "Ranking") {
        myLineChart.options.legend = {
            display: true,
            position: 'bottom'
        }
        var keys = Object.keys(LineChartDataGroupRanking[activity])
        datasets = []
        yscale = 0
        colors = [
            "rgba(128, 0, 0, 1)",
            "rgba(250, 190, 212, 1)",
            "rgba(245, 130, 49, 1)",
            "rgba(128, 128, 0, 1)",
            "rgba(225, 225, 25, 1)",
            "rgba(66, 212, 244, 1)",
            "rgba(0, 0, 117, 1)",
            "rgba(145, 30, 180, 1)"
        ]
        for (i = 0; i < keys.length; i++) {
            var key = keys[i]
            var dataLst = LineChartDataGroupRanking[activity][key]
            console.log(i, key, dataLst)

            datasets.push({
                label: key,
                data: dataLst,
                order: i + 1,
                lineTension: 0.3,
                backgroundColor: "rgba(255, 255, 255, 1)",
                borderColor: colors[i],
                pointRadius: 3,
                pointBackgroundColor: colors[i],
                pointBorderColor: colors[i],
                pointHoverRadius: 3,
                pointHoverBackgroundColor: colors[i],
                pointHoverBorderColor: colors[i],
                pointHitRadius: 10,
                pointBorderWidth: 2
            })

            num = Math.max.apply(null, dataLst) + 1
            if (num > yscale) yscale = num
        }
    } else {
        var datasets = [{
            label: "Activities",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: LineChartData[groupName][activity]
        }]  

        yscale = Math.max.apply(null, LineChartData[groupName][activity]) + 1
    }

    if (yscale > 6) yscale = 5; 
    myLineChart.options.scales["yAxes"][0].ticks.maxTicksLimit = yscale
    myLineChart.data.datasets = datasets;
    myLineChart.update()
    changeInnerHtml(groupName)
    //console.log()
}

function changeInnerHtml(groupName) {
    //LEAVE GROUP MODAL
    var x = document.getElementById("leaveGroupText")
    x.innerHTML = `Are you sure you want to leave '${groupName}'?`
    var x = document.getElementById("leaveGroupName")
    x.value = groupName

    //DELETE GROUP MODAL
    var x = document.getElementById("deleteGroupText")
    x.innerHTML = `Are you sure you want to delete '${groupName}'?`
    var x = document.getElementById("deleteGroupName")
    x.value = groupName

    //ADD MEMBER TO GROUP MODAL
    var x = document.getElementById("addMemberGroupName")
    x.value = groupName

    //REMOVE MEMBER TO GROUP MODAL
    var x = document.getElementById("removeMemberGroupName")
    x.value = groupName
    var x = document.getElementById("UserToRemove")
    x.innerHTML = ""; //REMOVE CURRENT OPTIONS
    for (var lst in groupDictionairy[groupName]) {
        var innerOption = document.createElement("option");
        var obj = groupDictionairy[groupName][lst];
        if (obj["pk_UserID"] != userId){
            var name = `${obj["firstName"]} ${obj["lastName"]}`;
            innerOption.value = obj["pk_UserID"];
            innerOption.innerHTML = name;
            x.appendChild(innerOption);
        }
    }

    //ADMIN TRANSFER GROUP MODAL
    var x = document.getElementById("transferAdminGroupName")
    x.value = groupName
    var x = document.getElementById("UserToTransfer")
    x.innerHTML = ""; //REMOVE CURRENT OPTIONS
    for (var lst in groupDictionairy[groupName]) {
        var innerOption = document.createElement("option");
        var obj = groupDictionairy[groupName][lst];
        if (obj["pk_UserID"] != userId){
            var name = `${obj["firstName"]} ${obj["lastName"]}`;
            innerOption.value = obj["pk_UserID"];
            innerOption.innerHTML = name;
            x.appendChild(innerOption);
        }
    }
}