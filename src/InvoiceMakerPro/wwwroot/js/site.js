var URL_SAVE_INVOICE = "/Invoices/SaveInvoice";
var URL_PRINT_INVOICE = "/Invoices/Print";
var URL_EDIT_INVOICE = "/Invoices/EditInvoice";
var URL_NEW_INVOICE = "/Invoices/EditInvoice";
var URL_RELEASE_INVOICE = "Invoices/ReleaseInvoice";
var URL_GETCUSTOMERINVOICES_URL = "Invoices/GetCustomerInvoices";

$(document).ready(function () {
    setupHeader();
    setupKeys();
});

function setupHeader() {
    $("#menu").kendoMenu();
}

function setupKeys() {
    $(document.body).keydown(function (e) {
        var grid = $("#grid").data("kendoGrid");
        if (grid !== undefined && grid !== null) {
            if (e.altKey && e.keyCode === 87)
                grid.table.focus();

            if (e.altKey && e.keyCode === 65)
                $(".k-grid-add").trigger("click");

            if (e.altKey && e.keyCode === 81) {
                var current = grid.current();
                if (current != null)
                    current.closest("tr").find(".k-grid-delete").trigger("click");
            }

            if (e.ctrlKey && e.keyCode === 83) {
                $("#SaveBtn").trigger("click");
                e.preventDefault();
            }

            if (e.ctrlKey && e.keyCode === 85) {
                $("#SaveAndNewBtn").trigger("click");
                e.preventDefault();
            }

            if (e.ctrlKey && e.keyCode === 81) {
                $("#CancelBtn").trigger("click");
                e.preventDefault();
            }
        }
    });
}

function parameterWrapFunction(options, operation) {
    if (operation !== "read" && options) {
        return { InvoiceId: options.InvoiceId };
    }
}

function editInvoice(e) {
    var grid = $("#gridInvoices").data("kendoGrid");
    var invoiceID = grid.dataItem($(e.currentTarget).closest("tr")).InvoiceId;
    window.location = URL_EDIT_INVOICE + (invoiceID != undefined ? ("/" + invoiceID) : '');
}

function printFromGrid(e) {
    var grid = $("#gridInvoices").data("kendoGrid");
    var invoiceID = grid.dataItem($(e.currentTarget).closest("tr")).InvoiceId;
    window.open(URL_PRINT_INVOICE + "/" + invoiceID, '_blank');
}

function releaseInvoice(e) {
    if (confirm("Are you sure you want to release this invoice? Released invoices cannot be modified or deleted.")) {
        var grid = $("#gridInvoices").data("kendoGrid");
        var invoiceID = grid.dataItem($(e.currentTarget).closest("tr")).InvoiceId;
        $.ajax({
            url: URL_RELEASE_INVOICE,
            data: { InvoiceId: invoiceID },
            success: function (result) {
                grid.dataSource.read();
                grid.refresh();
            },
            error: function (result) {
                alert(result.error);
            }
        });
    }
}

function newInvoice(e) {
    window.location = URL_NEW_INVOICE;
}

function onDataBoundEditInvoice(e) {
    $("tr .k-grid-delete", "#grid").on("click", function (e) {
        setTimeout(
            function () {
                updateGridTotals();
            }, 100);
    });

    //$("tr .checkbox", "#grid").on("click", function (e) {
    //    var checked = this.checked;
    //    var grid = $("#grid").data("kendoGrid");
    //    var model = grid.dataItem(this.closest("tr"));
    //    var dd = $("#Customer").data("kendoDropDownList");
    //    var selectedCustomer = dd.dataItem();
    //    //alert(selectedCustomer.VAT);
    //    //if (checked) {
    //    //    model.VAT = selectedCustomer.VAT;
    //    //    model.set("VAT", model.VAT);
    //    //}
    //    //else {
    //    //    model.VAT = model.Article.VAT;
    //    //    model.set("VAT", model.VAT);
    //    //}
        
    //});
}

function updateGridTotals() {
    var VATAmount = 0, NetTotal = 0, AdvancePaymentTaxAmount = 0, TotalWithVAT = 0, TotalToPay = 0;

    $.each($("#grid").data("kendoGrid").dataSource.data(), function (i, row) {
        NetTotal += row.Total;
        TotalWithVAT += row.TotalPlusVat;
    });
    AdvancePaymentTaxAmount = TotalWithVAT * ($("#AdvancePaymentTax").data("kendoNumericTextBox").value() / 100);
    VATAmount = TotalWithVAT - NetTotal;
    TotalToPay = TotalWithVAT - AdvancePaymentTaxAmount;

    $("#VATAmount").html(kendo.toString(VATAmount, "n2") + " NOK");
    $("#TotalWithVAT").html(kendo.toString(TotalWithVAT, "n2") + " NOK");
    $("#TotalToPay").html(kendo.toString(TotalToPay, "n2") + " NOK");
}

function updateRowAmounts(model) {
    if (model.Article != null) {
        model.Total = model.Qty * model.Price;
        model.TotalPlusVat = (1 + model.Article.VAT / 100) * model.Total;
        model.VatAmount = model.TotalPlusVat - model.Total;

        updateGridTotals();
    }
}

function onEditGrid(e) {
    setTimeout(function () {
        e.container.find("input").select();
    }, 100);
    e.model.unbind("change", function (event) { model_change(event, e.sender); }).bind("change", function (event) { model_change(event, e.sender); });
}

function onChangeAdvancePayment(e) {
    updateGridTotals();
}

function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}

function printInvoice(e) {
    var invoiceID = $("#invoiceId").val();
    window.open(URL_PRINT_INVOICE + "/" + invoiceID, '_blank');
}

function cancelInvoice(e) {
    location.reload();
}

function saveInvoiceTemplate(loadNew) {
    var dataInvoice = {
        "InvoiceNumber": $("#InvoiceNumber").html(),
        "InvoiceId": $("#invoiceId").val(),
        "Customer": $("#Customer").data("kendoDropDownList").dataItem(),
        "Store": $("#Store").data("kendoDropDownList").dataItem(),
        "TimeStamp": $("#TimeStamp").data("kendoDatePicker").value(),
        "DueDate": $("#DueDate").data("kendoDatePicker").value(),
        "AdvancePaymentTax": $("#AdvancePaymentTax").data("kendoNumericTextBox").value(),
        "Notes": "",
        "InvoiceDetails": []
    };

    if (dataInvoice.Customer.CustomerId === "")
        dataInvoice.Customer = null;

    if (dataInvoice.Store.StoreId === "")
        dataInvoice.Store = null;

    $.each($("#grid").data("kendoGrid").dataSource.data(), function (i, row) {
        if (row.InvoiceDetailsId === "")
            row.InvoiceDetailsId = guid();

        dataInvoice.InvoiceDetails.push({
            InvoiceDetailsId: row.InvoiceDetailsId,
            Article: row.Article,
            Price: row.Price,
            Qty: row.Qty,
            VAT: row.VAT,
            VatAmount: row.VatAmount,
            TotalPlusVat: row.TotalPlusVat
        });
    });

    $.ajax({
        url: URL_SAVE_INVOICE,
        type: "POST",
        contentType: 'application/json;',
        dataType: 'json',
        data: JSON.stringify(dataInvoice),
        success: function (result) {
            if (result.success === false) {
                alert(result.message);
            }
            else if (loadNew)
                window.location = URL_NEW_INVOICE;
            else {
                alert("Invoice saved.");
                var printBtn = $("#PrintBtn").data("kendoButton");
                printBtn.enable(true);
                $("#invoiceId").val(result.id);
            }
        },
        error: function (result) {
            alert(result.error);
        }
    });
}

function saveInvoice(e) {
    saveInvoiceTemplate(false);
}

function saveAndNewInvoice(e) {
    saveInvoiceTemplate(true);
}

function onChangeArticle(e) {
    var grid = $("#grid").data("kendoGrid");
    var current = grid.tbody.find(".k-edit-cell");
    if (current.length === 0)
        current = grid.current();
    var model = grid.dataItem(current.closest("tr"));
    if (typeof model.Article != 'object') {
        model.Article = null;
        return;
    }
    setLastEditedCell(grid);

    model.Price = model.Article.Price;
    model.VAT = model.Article.VAT;
    updateRowAmounts(model);
    grid.refresh();
    if (CURRENT_CELL != null) {
        setTimeout(function () {
            JumpNextEditedCell(grid);
        }, 0);
    }
}

function model_change(e, grid) {
    var field = e.field;
    if (field === "Qty" || field === "Price") {
        setLastEditedCell(grid);
        updateRowAmounts(e.sender.source != null ? e.sender.source : e.sender);
        setTimeout(function () {
            grid.refresh();
            JumpNextEditedCell(grid);
        }, 0);
    }
}

function onDataBoundGridTemplate(e) {
    // hide edit, release delete and print buttons if InvoiceState is Released
    var grid = e.sender;
    var gridData = grid.dataSource.view();

    for (var i = 0; i < gridData.length; i++) {
        var currentUid = gridData[i].uid;
        if (gridData[i].InvoiceState === 1) {
            var currenRow = grid.table.find("tr[data-uid='" + currentUid + "']");
            $(currenRow).find(".k-grid-editItem").hide();
            $(currenRow).find(".k-grid-release").hide();
            $(currenRow).find(".k-grid-delete").hide();
        }
    }

    e.sender.tbody.find(".k-button.k-grid-editItem").each(function (idx, element) {
        $(element).find("span").addClass("k-icon k-edit");
    });

    e.sender.tbody.find(".k-button.k-grid-release").each(function (idx, element) {
        $(element).find("span").addClass("k-icon k-i-lock");
    });

    e.sender.tbody.find(".k-button.k-grid-deleteItem").each(function (idx, element) {
        $(element).find("span").addClass("k-icon k-delete k-i-delete");
    });

    e.sender.tbody.find(".k-button.k-grid-viewItem").each(function (idx, element) {
        $(element).find("span").addClass("k-icon k-si-arrow-e");
    });

    e.sender.tbody.find(".k-button.k-grid-print").each(function (idx, element) {
        $(element).find("span").addClass("fa fa-print small-margin-right");
    });
}

function actionOnRowItem(e, gridName, PropertyId, UrlAction) {
    var grid = $(gridName).data("kendoGrid");
    var propid = grid.dataItem($(e.currentTarget).closest("tr"))[PropertyId];
    window.location = UrlAction + (propid != undefined ? ("/" + propid) : '');
}

function searchGrid() {
    var searchValue = $("#searchBox").val();
    var kgrid = $("#gridInvoices").data("kendoGrid");
    searchValue = searchValue.toUpperCase();
    var searchArray = searchValue.split(" ");
    if (searchValue) {
        var orfilter = { logic: "or", filters: [] };
        var andfilter = { logic: "and", filters: [] };
        $.each(searchArray, function (i, v) {
            if (v.trim() == "") {
            }
            else {
                $.each(searchArray, function (i, v1) {
                    if (v1.trim() == "") {
                    }
                    else {
                        orfilter.filters.push({ field: "Customer.Name", operator: "contains", value: v1 },
                                              { field: "Store.Name", operator: "contains", value: v1 },
                                              { field: "InvoiceDetails", 
                                              operator: function (item, value) {
                                                  var array = Array.from(item);
                                                  for (var i = 0; i < array.length; i++) {
                                                      var artName = array[i].Article.Name.toUpperCase();
                                                      if (artName.includes(value))
                                                          return true;
                                                  }
                                                  return false;
                                              }, 
                                                value: v1 });
                        andfilter.filters.push(orfilter);
                        orfilter = { logic: "or", filters: [] };
                    }
                });
            }
        });
        kgrid.dataSource.filter(andfilter);
    }
    else {
        kgrid.dataSource.filter({});
    }
}

// generate client template function to display each invoice's articles on the Index page grid
function generateTemplate(InvoiceDetails) {
    var invoiceDetails = Array.from(InvoiceDetails);
    var template = "<ul>";
    for (var i = 0; i < InvoiceDetails.length; i++) {
        template = template + "<li>" + InvoiceDetails[i].Article.Name + "</li>";
    }

    return template + "</ul>";
}

function switchVAT(e) {
    var grid = $("#grid").data("kendoGrid");
    var model = grid.dataItem($(e.currentTarget).closest("tr"));
    var dd = $("#Customer").data("kendoDropDownList");
    var customerVAT = dd.dataItem().VAT;
    var articleVAT = model.Article.VAT;
    //var toggle = $(e.currentTarget).text();

    // toggle between VATs
    if (model.VAT === articleVAT && customerVAT != undefined) {
        model.VAT = customerVAT;
        //$(e.currentTarget).text("Using Customer VAT");
    }
    else if (articleVAT != undefined) {
        model.VAT = articleVAT;
        //$(e.currentTarget).text("Using Article VAT");
    }
    grid.refresh();
}

function onSelectCustomer(e) {
    var dd = $("#Customer").data("kendoDropDownList");
    var value = dd.dataItem().VAT;
    $("#CustomerVATLabel").val(value);
}