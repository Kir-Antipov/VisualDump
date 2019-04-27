$("body").on("mouseenter", ".value", function() {
    $(".name").removeClass("selected");
    $(".value").removeClass("selected");
    if (!$(this).find("table").length)
        $(this).addClass("selected");
    $(this).parent().find(".name").addClass("selected");
    var header = $(this).parent().parent().find(".row-header");
    if (header.length)
    {
        var cellIndex = $(this).parent().children().index(this);
        header.find(".name").eq(cellIndex).addClass("selected");
    }
});        

$("body").on("mouseleave", ".value", function() {
    $(this).removeClass("selected");
    $(this).parent().find(".name").removeClass("selected");
    var header = $(this).parent().parent().find(".row-header");
    if (header.length)
    {
        var cellIndex = $(this).parent().children().index(this);
        header.find(".name").eq(cellIndex).removeClass("selected");
    }
});