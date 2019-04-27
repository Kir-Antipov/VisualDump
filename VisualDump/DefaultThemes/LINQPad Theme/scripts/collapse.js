$("body").on("click", ".title-box__collapse", function() {
    $(this).toggleClass("show").parent().parent().find("table").first().toggleClass("hidden");
});  