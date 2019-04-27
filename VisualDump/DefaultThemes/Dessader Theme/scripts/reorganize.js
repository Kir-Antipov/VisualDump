function intersection(arr) {
    var result = [];
    if (arr.length) {
        for (var i = 0; i < arr[0].length; ++i)
        {
            var all = true;
            for (var j = 1; j < arr.length; ++j)
                if (arr[j].indexOf(arr[0][i]) == -1) {
                    all = false;
                    break;
                }
            if (all)
                result.push(arr[0][i]);
        }
    }
    return result;
}

var currentCacheTBodyID = 0;
var cacheTBody = [];

$("body").on("click", ".title-box__reorganize", function() {
    var $currentTable = $(this).parent().parent().children("table");
    var id = $currentTable.attr("data-cache_tbody_id");
    var state = $currentTable.attr("data-cache_tbody_state");
    if (state === undefined)
        state = 0;
    state = Number(state);
    $currentTable.attr("data-cache_tbody_state", Number(!state));
    var newBody = "";
    if (state) {
        newBody = cacheTBody[Number(id)].oldBody;
    } else
    if (id !== undefined) {
        newBody = cacheTBody[Number(id)].newBody;
    } else
    {
        id = currentCacheTBodyID++;
        var allNames = [];
        var dataStorage = { length: 0 };
        $currentTable.attr("data-cache_tbody_id", id);
        $currentTable.children("tbody").children(".row").each(function(ir, row) {
            var currentNames = [];
            $(row).children(".value").children(".table-wrap").children("table").children("tbody").children(".row").each(function(irr, innerRow) {
                $(innerRow).children(".name").each(function(iname, name) { 
                    var text = $(name).text(); 
                    currentNames.push(text);  
                    if (!dataStorage[ir])
                        dataStorage[ir] = {};
                    dataStorage[ir][text] = $(name).parent().children(".value").first().html();
                });
            });
            dataStorage.length++;
            allNames.push(currentNames);
        });
        
        newBody = "<tr class='.row-header'>";
        var intersectedNames = intersection(allNames);
        for (var i = 0; i < intersectedNames.length; ++i)
            newBody += "<td class='name'>" + intersectedNames[i] + "</td>";
        newBody += "</tr>";
        for (var i = 0; i < dataStorage.length; ++i)
        {
            newBody += "<tr class='row'>";
            for (var j = 0; j < intersectedNames.length; ++j)
                newBody += "<td class='value'>" + dataStorage[i][intersectedNames[j]] + "</td>";
            newBody += "</tr>";
        }
        cacheTBody[id] = { oldBody: $currentTable.children("tbody").html(), newBody: newBody };
    }
    $currentTable.children("tbody").html(newBody);       
});