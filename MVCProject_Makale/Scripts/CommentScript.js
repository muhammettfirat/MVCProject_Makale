var notid = -1;
$(function () {
    $('#ModalComment').on('show.bs.modal', function (e) {
        notid = $(e.relatedTarget).data("notid");   //related target butonu yakalamak için
        $("#modal-body").load("/Comment/YorumGoster/" + notid);
    });
})

function yorumislem(btn, islem, yorumid, spanid) {
    var mode = $(btn).data("edit-mode")
    if (islem == "editislem") {
        if (!mode) {   //mode true ise  //istiyorum ki edit butonu kalksın yerine tik butonu gelsin
            $(btn).data("edit-mode", true) //butonda edit modu true yap
            $(btn).removeClass("btn-warning")
            $(btn).addClass("btn-success")
            var btnSpan = $(btn).find("span") //butonda span i bul
            btnSpan.removeClass("glyphicon-edit")  //edit i sil
            btnSpan.addClass("glyphicon-ok")   //ok u ekle

            $(spanid).addClass("editable")
            $(spanid).attr("contenteditable", true) //bunu true yap
            $(spanid).focus()  //düzenlemeyi direkt orda yazıyım baska bir yere yönlendirmesin 
        }
        else {   //geri döndüğümde eski haline gelsin istiyorum
            $(btn).data("edit-mode", false) //butonda edit modu true yap
            $(btn).removeClass("btn-success")
            $(btn).addClass("btn-warning")
            var btnSpan = $(btn).find("span") //butonda span i bul
            btnSpan.removeClass("glyphicon-ok")  //ok u sil
            btnSpan.addClass("glyphicon-edit")   //editi ekle

            $(spanid).removeClass("editable")
            $(spanid).attr("contenteditable", false) //bunu true yap
            var txt = $(spanid).text()
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + yorumid,
                data: { text: txt }
            }).done(function (data) {
                if (data.sonuc) {   //update gerçekleştiyse yorumlar tekrar yüklenir
                    $("#modal-body").load("/Comment/YorumGoster/" + notid);
                }
                else {
                    alert("Yorum güncellenemedi.")
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.")
            })
        }
    }
    else if (islem == "deleteislem") {
        var mesaj = confirm("Yorum silinsin mi?")
        if (!mesaj) {
            return false;
        }
        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + yorumid
        }).done(function (data) {
            if (data.sonuc) {   //silme gerçekleştiyse yorumlar tekrar yüklenir
                $("#modal-body").load("/Comment/YorumGoster/" + notid);
            }
            else {
                alert("Yorum silinemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        })
    }
    else if (islem == "insertislem") {
        var txt = $("#yorum_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { "text": txt, "notid": notid }
        }).done(function (data) {
            if (data.sonuc) {   //ekleme gerçekleştiyse yorumlar tekrar yüklenir
                $("#modal-body").load("/Comment/YorumGoster/" + notid);
            }
            else {
                alert("Yorum eklenemedi.")
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.")
        })
    }
}