$(function () {

    var noteids = [];

    $("div[data-note-id]").each(function (i, e) {

        noteids.push($(e).data("note-id"));

    });
    console.log(noteids);
    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {

        console.log(data);
        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.children().first();

                btn.data("liked", true);
                span.removeClass("far fa-star");
                span.addClass("fas fa-star");
            }
        }
    }).fail(function () {

    });



    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked }
        }).done(function (data) {

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);
                spanStar.removeClass("fas fa-star");
                spanStar.removeClass("far fa-star");

                if (liked) {
                    spanStar.addClass("fas fa-star");
                } else {
                    spanStar.addClass("far fa-star");
                }

            }

        }).fail(function () {
            alert("Connection to the server failed.");
        })

    });
});