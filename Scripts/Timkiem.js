$('#btntimkiem').on('click', function () {
    var giatrinhapvao = $('#searchcontent').val();
    if ($('#searchcontent').val() == " ") {
        alert("nhapp");
    }
    else {


        window.location.href = "/TrangChu/TimKiem?name=" + giatrinhapvao;
    }

});