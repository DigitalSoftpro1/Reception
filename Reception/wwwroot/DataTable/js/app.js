$(document).ready(function () {
  var table = $("#visitTable").DataTable();
  var treatmentTable = $("#treatmentTable").DataTable();

  function updateVisitCount() {
    var count = table.rows({ filter: "applied" }).data().length;
    $("#visitCount").text("Number of Visit: " + count);
  }
  updateVisitCount();

  // البحث بالتاريخ فقط
  $("#dateSearch").on("change", function () {
    var selectedDate = $(this).val();
    $.fn.dataTable.ext.search = [];
    if (selectedDate) {
      $.fn.dataTable.ext.search.push(function (settings, data) {
        var visitDate = data[3].split(" ")[0];
        var parts = visitDate.split("/");
        var formattedDate = parts[2] + "-" + parts[1] + "-" + parts[0];
        return formattedDate === selectedDate;
      });
    }
    table.draw();
    updateVisitCount();
  });

  table.on("draw", function () {
    updateVisitCount();
  });

  // عند الضغط مرتين على صف لفتح جدول العلاج
  // عند الضغط مرتين على صف لفتح جدول العلاج
  $("#visitTable tbody").on("dblclick", "tr", function () {
    var rowData = table.row(this).data();
    if (!rowData) return;

    // تحديث بيانات الأزرار
    $("#treatmentVisitDate").text(rowData[3]);
    $("#treatmentPatient").text(rowData[2]);
    $("#treatmentVisitNo").text(rowData[4]);

    // إظهار القسم
    $("#treatmentSection").show();

    // مسح الجدول السابق
    $("#treatmentTable tbody").empty();

    // إضافة بيانات تجريبية للجدول الثاني
    var treatments = [
      { name: "Treatment A", org: 100, pat: 80, refused: "No" },
      { name: "Treatment B", org: 150, pat: 120, refused: "Yes" },
      { name: "Treatment C", org: 200, pat: 160, refused: "No" },
    ];

    treatments.forEach(function (t) {
      var row =
        "<tr>" +
        "<td>" +
        t.name +
        "</td>" +
        "<td>" +
        t.org +
        "</td>" +
        "<td>" +
        t.pat +
        "</td>" +
        "<td>" +
        t.refused +
        "</td>" +
        "</tr>";
      $("#treatmentTable tbody").append(row);
    });
  });
  // عند الضغط مرتين على صف في جدول الثاني لعرض جدول الدفع
  $("#treatmentTable tbody").on("dblclick", "tr", function () {
    // مثال بيانات الدفع ثابتة (يمكن الربط ببيانات حقيقية)
    var payments = [
      { date: "20/12/2025 12:34", type: "Cash", value: "20.000" },
    ];

    // إظهار القسم
    $("#paymentSection").show();

    // مسح الجدول السابق
    $("#paymentTable tbody").empty();

    // إضافة بيانات الدفع
    payments.forEach(function (p) {
      var row =
        "<tr>" +
        "<td>" +
        p.date +
        "</td>" +
        "<td>" +
        p.type +
        "</td>" +
        "<td>" +
        p.value +
        "</td>" +
        "</tr>";
      $("#paymentTable tbody").append(row);
    });

    // يمكن هنا تحديث الملخصات المالية إذا أردت ديناميكيًا
    $("#invoiceTotal").text("47.000");
    $("#discount").text("0.00");
    $("#amountToPay").text("47.000");
    $("#totalPaid").text("20.000");
    $("#remain").text("27.000");
  });
});
