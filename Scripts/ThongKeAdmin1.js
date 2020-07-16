Thongke = (function () {
    var year = document.getElementById("year");
    var month = document.getElementById("month");
    var yearitem = year.options[year.selectedIndex].text;
    var monthitem = month.options[month.selectedIndex].text;
    window.location.href = "/ThongKe/ThongKe?month=" + monthitem + "&year=" + yearitem
    /// vi - vn /

    //$.ajax({
    //    url: "/ThongKe/ThongKe",
    //    data: { iyear: yearitem, imonth: monthitem },
    //    type: "POST",
    //    contentType: "Student",
    //    success: function (result) {

    //        $('.thongkehoadon').empty();

    //        var hinh = result.hinhminhhoa;
    //        var R =
    //            " < thead >"
    //            + "<tr>"
    //            + "<th>Mã hóa đơn</th>"
    //            + "<th>Mã khách hàng</th>"
    //            + "<th>Tên khách hàng</th>"
    //            + " <th>Điện thoại</th>"
    //            + "<th>Địa chỉ</th>"
    //            + "<th>Đơn giá</th>"
    //            + "<th>Ngày đặt</th>"
    //            + "<th>Ngày giao</th>"
    //            + "</tr>"
    //            + "</thead >"
    //            + "<tbody>"


    //            + "  <tr>"
    //            + " <td>" + result.madh + "</td>"
    //            + "<td>" + result.makh + "</td>"
    //            + " <td>" + result.tenkh + "</td>"
    //            + " <td>" + result.dienthoai + "</td>"
    //            + " <td>" + result.diachi + "</td>"
    //            + " <td>" + result.dongia + "</td>"
    //            + "<td>" + result.ngaydat + "</td>"
    //            + "<td>" + result.ngaygiao + "</td>"
    //            + " </tr>"
    //            + " }"

    //            + " </tbody>"
    //        "  < span > Tong Tien: " + result.x + "</span >"


    //        $('.thongkehoadon').append(R);

    //    }

    //});
    //return false;
});

HoaDon = (function () {
    var year = document.getElementById("nam");
    var month = document.getElementById("thang");
    var day = document.getElementById("ngay");
    var yearitem = year.options[year.selectedIndex].text;
    var monthitem = month.options[month.selectedIndex].text;
    var dayitem = day.options[day.selectedIndex].text;
    if (dayitem == "Chọn Ngày") {
        window.location.href = "/ThongKe/Hoadonthangthanhtoan?year=" + yearitem + "&month=" + monthitem
    }
    else if (monthitem == "Chọn Tháng" || dayitem == "Chọn Ngày") {
        window.location.href = "/ThongKe/Hoadonnamthanhtoan?year=" + yearitem

    }
    else {
        window.location.href = "/ThongKe/Hoadonngaythanhtoan?day=" + dayitem + "&year=" + yearitem + "&month=" + monthitem

    }
});