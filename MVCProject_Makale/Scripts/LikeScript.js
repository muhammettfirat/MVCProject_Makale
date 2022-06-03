$(function () {

    var notids = [];

    $("div[data-notid]").each(function (i, e) {
        notids.push($(e).data("notid"));
    });

    $.ajax({
        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: notids }
    }).done(function (data) {
        console.log(data.sonuc);
        if (data.sonuc !=null && data.sonuc.length>0)
        {
            for (var i = 0; i < data.sonuc.length; i++)
            {
                var id = data.sonuc[i];
                var likenot = $("div[data-notid=" + id + "]");
                var btn = likenot.find("button[data-liked]");
                var spanheart = btn.find("span.like-heart");

                btn.data("liked", true);
                spanheart.removeClass("glyphicon-heart-empty");
                spanheart.addClass("glyphicon-heart");
            }

        }

    }).fail(function (){

    });

    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var notid = btn.data("notid");
        var spanheart = btn.find("span.like-heart");
        var spancount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Note/SetLiked",
            data: { "notid": notid, "liked": !liked }
        }).done(function (data) {
            if (data.hata)
            {
                alert(data.mesaj);
            }                
            else
            {
                liked = !liked;
                btn.data("liked",liked);
                spancount.text(data.sonuc);

                spanheart.removeClass("glyphicon-heart-empty");
                spanheart.removeClass("glyphicon-heart");

                if (liked)
                {
                  spanheart.addClass("glyphicon-heart");
                }
                else
                {
                   spanheart.addClass("glyphicon-heart-empty");
                }                  

            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        });

    });




});