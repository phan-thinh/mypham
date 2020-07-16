$(document).ready(function () {
    DatHang = function (MaSP) {
        var SL = $('#txtSL').val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        $.ajax({
            url: "/GioHang/ThemGiohang",
            data: { iMaSP: MaSP, SL: SL },
            type: "POST",
            success: function (result) {
                var TongSL = 0;
                var TongTien = 0;
                $.each(result, function (i, item) {
                    TongSL += item.SoLuong;
                    TongTien += item.TongTien
                });
                $('#cart > a  > span').html(TongSL + " Sản Phẩm - " + TongTien + "VND")
                alert("Đặt Hàng Thành Công")
            },
        });
        return false;
    };
    CapNhat = (function (MaSP) {
        SL = $("#" + MaSP).val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        var TongTien1 = 0;
        var TongSL1 = 0
        $.ajax({
            url: "/GioHang/ThemGiohang",
            data: { iMaSP: MaSP, SL: SL },
            type: "POST",
            success: function (result) {
                var item = "";
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                        + "<td id='MaNuocHoa' name='MaNuocHoa' style='display: none;'>" + item.MaSP + "</td>"
                        + "<td><a href='/ViewNuocHoa/ChiTietNuocHoa?MaNuocHoa=@item.MaSP'><img src='/Content/Images/product/" + item.AnhSP + "' /></a></td>"
                        + "<td>" + item.TenSP + "</td>"
                        + "<td><input type='text' id='" + item.MaSP + "' class='txtSLGH' value=" + item.SoLuong + " /><a class='fa fa-upload btn-Update' onclick='CapNhat(" + item.MaSP + ")' aria-hidden='true'></a><a  class='fa fa-trash-o btn-Delete' aria-hidden='true' onclick=' XoaSP(" + item.MaSP + ")'></a></td>"
                        + "<td>" + item.GiaSP + "</td>"
                        + "<td>" + item.TongTien + "</td>"
                        + "</tr>"
                    TongTien1 += item.TongTien;
                    TongSL1 += item.SoLuong;
                    
                    $('.Cart-info tbody').append(R);
                });
                $('#TongTien > h2 > span').html(TongTien1)
                $('#cart > a  > span').html(TongSL1 + " Sản Phẩm - " + TongTien1 + "VND")
            },
        });
        return false;
    });
    XoaSP = (function (MaSP) {
        $.ajax({
            url: "/GioHang/XoaSP",
            data: { iMaSP: MaSP },
            type: "POST",
            success: function (result) {
                var item = "";
                var TongTien = 0;
                var TongSL2 = 0;
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                        + "<td id='MaNuocHoa' name='MaNuocHoa' style='display: none;'>" + item.MaSP + "</td>"
                        + "<td><a href='/ViewNuocHoa/ChiTietNuocHoa?MaNuocHoa=@item.MaSP'><img src='/Content/Images/product/" + item.AnhSP + "' /></a></td>"
                        + "<td>" + item.TenSP + "</td>"
                        + "<td><input type='text' id='" + item.MaSP + "' class='txtSLGH' value=" + item.SoLuong + " /><a class='fa fa-upload btn-Update' onclick='CapNhat(" + item.MaSP + ")' aria-hidden='true'></a><a  class='fa fa-trash-o btn-Delete' aria-hidden='true'onclick=' XoaSP(" + item.MaSP + ")'></a></td>"
                        + "<td>" + item.GiaSP + "</td>"
                        + "<td>" + item.TongTien + "</td>"
                        + "</tr>"
                    TongTien += item.TongTien
                    TongSL2 += item.SoLuong
                    $('.Cart-info tbody').append(R);

                });
                $('#TongTien > h2 > span').html(TongTien)
                $('#cart > a  > span').html(TongSL2 + " Sản Phẩm - " + TongTien + "VND")
            },
        });
        return false;
    });
    tai = function () {
        alert("Load lại")
    };

    GiamGia = (function () {
        var txtGiamGia = $('#txtmagiamgia').val();
        var TongTien1 = 0;
        var TongSL1 = 0
        $.ajax({
            url: "/GioHang/GiamGia",
            data: { iMaSP: txtGiamGia},
            type: "POST",
            success: function (result) {
                alert(txtGiamGia);
                var item = "";
                $('.giamgia').empty();
                $.each(result, function (i, item) {
                    TongTien1 += item.TongTien;
                    TongSL1 += item.SoLuong;
                    alert(txtGiamGia);
                    $('.giamgia').append(R);
                });
                $('#TongTien > h2 > span').html(txtGiamGia)
                $('#cart > a  > span').html(TongSL1 + " Sản Phẩm - " + TongTien1 + "VND")
            },
        });
        return false;
    });
});
