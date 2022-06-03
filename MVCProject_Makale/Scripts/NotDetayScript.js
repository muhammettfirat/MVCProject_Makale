
$(function () {

    $('#ModalNote').on('show.bs.modal', function (e) {
        var notid = $(e.relatedTarget).data("notid");   //related target butonu yakalamak için
        $("#modal-body").load("/Note/NotDetay/" + notid);
    });

   
});