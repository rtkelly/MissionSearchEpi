


$(".sort-by").change(function () {

    var sortby = $(this).val()
    var href = window.location.href

    if (href.indexOf("sort=") > -1) {
        href = href.replace(/&sort=[^&]*/g, "")
    }

    window.location.href = href + "&sort=" + sortby
});

