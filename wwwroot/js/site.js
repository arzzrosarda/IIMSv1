
function InMainLoader() {
    $(".sub-loader").fadeIn(100);
}
function OutMainLoader() {
    $(".sub-loader").fadeOut(100);
}

$("#btnChangePassword").on("click", function () {
    Swal.fire({
        title: "CHANGE PASSWORD",
        html: "Are you sure you want to change your password?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            $("form[name='ChangePasswordForm']").submit();
        }
    });
});
$("#btnSaveChangesEditInfo").on("click", function () {
    Swal.fire({
        title: "SAVE CHANGES?",
        html: "Are you sure you want to update this user information?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            $("form[name='editUserInfoForm']").submit();
        }
    });
});
$("#btnResetCredentials").on("click", function () {
    Swal.fire({
        title: "RESET CREDENTIALS",
        html: "Are you sure you want to reset this user credentials?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            $("form[name='ResetCredentialsForm']").submit();
        }
    });
});

$(".btnDeactiveInStock").on("click", function () {
    Swal.fire({
        title: "IN STOCK",
        html: "THE ITEM IS ALREADY IN STOCK <br/>disabling the item is prohibited!",
        icon: "warning",
        showCancelButton: true,
        showConfirmButton: false,
        cancelButtonText: "Close",
        closeOnConfirm: true,
        closeOnCancel: true
    });
}); 
$(".btnCancelForApproval").on("click", function () {
    var relId = $(this).attr("data-releaseId");
    Swal.fire({
        title: "PULLOUT",
        html: "Are you sure you want to pullout the release for this item?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            InMainLoader();
            $.ajax({
                url: "/Records/_CancelRelease",
                method: "Post",
                data: { RelId: relId },
                success: function (response) {
                    window.location.reload();
                }
            });
        }
    });
});
$("#btnDeptCheckifSel").on("click", function () {
    var value = $("#DepartmentSel").val();
    if (value != ""  ) {
        $("#btnSubmitReleaseItemBulk").click();
    }
    else {
        $("#DepartmentSel").focus();
        $("#DeptSelTextValidation").text("The department field is required!");
    }
});
$("#btnSubmitReleaseItemBulk").on("click", function () {
    var deptSel = $("#DepartmentSel").val();
    Swal.fire({
        title: "RELEASE ITEM",
        html: "Are you sure you inserted all needed items and information <br/> Ready to release this items?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            if (deptSel != "") {
                
                $("#btnSubmitFormRelease").click();
                window.onbeforeunload = function (event) {
                    InMainLoader();
                };
            }
            else {
                $("#DepartmentSel").focus();
            }
        } 
    });
});
$("#btnSubmitAddStock").on("click", function () {
    Swal.fire({
        title: "NEW STOCK",
        html: "Are you sure you want to add stocks in this item?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            $("#submitNewAddStock").click();
        }
    });
});

$("#updateItem").on("click", function () {
    Swal.fire({
        title: "UPDATE",
        html: "Are you sure you want to update this item?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            var container = $("#SubmitUpdateItemBtnContainer");
            container.empty();
            var btn = "";
            btn += '<button class="btn btn-primary w-100" type="submit" id="updateItemBtn" style="display: none;">Update Item</button>';
            container.html(btn);
            $("#updateItemBtn").click();
            InMainLoader();
        }
    });
});

$("#SaveItem").on("click", function () {
    Swal.fire({
        title: "NEW ITEM",
        html: "Are you sure you want to save this item?",
        icon: "info",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            var container = $("#SubmitAddItemBtnContainer");
            container.empty();
            var btn = "";
            btn += '<button class="btn btn-primary w-100" type="submit" id="AddItemBtn" style="display: none;">Save Item</button>';
            container.html(btn);
            $("#AddItemBtn").click();
            InMainLoader();
        }
    });
});
$("#logBtn").on("click", function () {
    Swal.fire({
        title: "LOGOUT",
        html: "Are you sure you want to logout?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#1c3d77",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            let timerInterval;
            Swal.fire({
                title: "LOGGING OUT....",
                html: "",
                timer: 1000,
                icon: "warning",
                confirmButtonColor: "#1c3d77",
                showConfirmButton: false,
                timerProgressBar: true,
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                    const timer = Swal.getPopup().querySelector("b");
                    timerInterval = setInterval(() => {
                        timer.textContent = `${Swal.getTimerLeft()}`;
                    }, 100);
                },
                willClose: () => {
                    clearInterval(timerInterval);
                }
            }).then((result) => {
                if (result.dismiss === Swal.DismissReason.timer) {
                    document.getElementById("logoutbtn").click();
                }
            });

        }
    });
});
$(document).ready(function () {
    $("#releasedListTbl").DataTable({
        scrollY: '450px',
        scrollCollapse: true,
        'ordering': false,
        paging: false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false
    });
    
   
    $("#releasesRecord").DataTable({
        /* pageLength: 8,*/
        //columnDefs: [
        //    { orderable: false, className: 'reorder', targets: 0 },
        //    { orderable: true, targets: '_all' }
        //],
        //scrollY: '430px',
        //scrollX: false,
        //scroller: false,
        //scrollCollapse: true,
        "ordering": false,
        paging: false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false
    });
    
    $("#stocksTbl").DataTable({
        "ordering": false,
        paging: false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false
    }); 
    
    $("#itemTbl").DataTable({
        "ordering": false,
        paging: false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false
    });

    $("#supplyTable").DataTable({
        pageLength: 10,
        layout: {
            topStart: {
                buttons: [
                    {
                        text: '<span class="text-success"><i class="fas fa-plus-circle"></i> &nbsp;</span> Add Supply Type',
                        className: 'btn btn-light',
                        tag: 'span',
                        action: function (e, dt, node, config) {
                            $("#modal-add-supply").modal("show");
                        }
                    }
                ]
            },
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        },
        //language: {
        //    paginate: {
        //        first: '<i class="fas fa-angle-double-left" style="font-size: 13px;"></i>',
        //        next: '&nbsp;<i class="fas fa-angle-right" style="font-size: 13px;"></i>',
        //        previous: '<i class="fas fa-angle-left" style="font-size: 13px;"></i>&nbsp;',
        //        last: '<i class="fas fa-angle-double-right" style="font-size: 13px;"></i>',
        //    }
        //},
        language: {
            paginate: {
                next: 'Next',
                previous: 'Previous',
            }
        },
        paging: true,
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": false
    });
    $("#unitTable").DataTable({
        pageLength: 10,
        layout: {
            topStart: {
                buttons: [
                    {

                        text: '<span class="text-success"><i class="fas fa-plus-circle"></i> &nbsp;</span> Add Unit Type',
                        className: 'btn btn-light',
                        tag: 'span',
                        action: function (e, dt, node, config) {
                            $("#modal-add-unit").modal("show");
                        }
                    }
                ]
            },
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        },
        language: {
            paginate: {
                next: 'Next',
                previous: 'Previous',
            }
        },
        paging: true,
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": false
    });

    $("#specsTable").DataTable({        
        pageLength: 10,
        layout: {
            topStart: {
                buttons: [
                    {
                        text: '<span class="text-success"><i class="fas fa-plus-circle"></i> &nbsp;</span> Add Description Type',
                        className: 'btn btn-light',
                        tag: 'span',
                        action: function (e, dt, node, config) {
                            $("#modal-add-specs").modal("show");
                        }
                    }
                ]
            },
             bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        },
        language: {
            paginate: {
                next: 'Next',
                previous: 'Previous',
            }
        },
        paging: true,
        "bPaginate": true,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": false
    }); 

    $("#ReportTableItem").DataTable({
        /*pageLength: 5,*/
        scrollY: '430px',
        scrollX: true,
        scrollCollapse: true,
        ordering: false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": false,
        "bAutoWidth": false
    });
});

$("[data-checkboxes]").each(function () {
    var me = $(this),
        group = me.data("checkboxes"),
        role = me.data("checkbox-role");

    me.change(function () {
        var Container = $("#ItemsSelContainer");
        var itemChecked = $("[data-checkboxes]:checked");
        var array = [];
        for (var i = 0; i < itemChecked.length; i++) {
            array.push(itemChecked[i].value);
        }
        Container.val(array.toString());
        var all = $("[data-checkboxes=" + group + "]:not([data-checkbox-role=dad])"),
            checked = $("[data-checkboxes=" + group + "]:not([data-checkbox-role=dad]):checked"),
            dad = $("[data-checkboxes=" + group + "][data-checkbox-role=dad]"),
            total = all.length,
            checked_length = checked.length;

        if (role == "dad") {
            if (me.is(":checked")) {
                $(".btnDisabledItemChecked").prop("disabled", false);
                $(".btnEnabledItemChecked").prop("disabled", false);
                $(".btnDeleteItemChecked").prop("disabled", false);

                me.prop("checked", true);
            } else {
                $(".btnDisabledItemChecked").prop("disabled", true);
                $(".btnEnabledItemChecked").prop("disabled", true);
                $(".btnDeleteItemChecked").prop("disabled", true);
                me.prop("checked", false);
            }
            if (dad.is(":checked")) {
                $(".btnDisabledItemChecked").prop("disabled", false);
                $(".btnEnabledItemChecked").prop("disabled", false);
                $(".btnDeleteItemChecked").prop("disabled", false);

                all.prop("checked", true);
                all.each(function () {
                    var val = $(this).attr("data-itemId");
                    $("#tr_" + val).addClass("bg-light");
                });
            } else {
                $(".btnDisabledItemChecked").prop("disabled", true);
                $(".btnEnabledItemChecked").prop("disabled", true);
                $(".btnDeleteItemChecked").prop("disabled", true);

                all.prop("checked", false);
                all.each(function () {
                    var val = $(this).attr("data-itemId");
                    
                    $("#tr_" + val).removeClass("bg-light");
                });
            }
        } else {
            if (checked_length >= total) {
                $(".btnDisabledItemChecked").prop("disabled", false);
                $(".btnEnabledItemChecked").prop("disabled", false);
                $(".btnDeleteItemChecked").prop("disabled", false);

                dad.prop("checked", true);
            } else {
                $(".btnDisabledItemChecked").prop("disabled", false);
                $(".btnEnabledItemChecked").prop("disabled", false);
                $(".btnDeleteItemChecked").prop("disabled", false);

                dad.prop("checked", false);
            }

            all.each(function () {
                    var ethis = $(this);
                    if (ethis.is(":checked")) {
                        $(".btnDisabledItemChecked").prop("disabled", false);
                        $(".btnEnabledItemChecked").prop("disabled", false);
                        $(".btnDeleteItemChecked").prop("disabled", false);

                        var val = ethis.attr("data-itemId");
                        $("#tr_" + val).addClass("bg-light");
                    } else {
                        $(".btnDisabledItemChecked").prop("disabled", true);
                        $(".btnEnabledItemChecked").prop("disabled", true);
                        $(".btnDeleteItemChecked").prop("disabled", true);

                        var val = ethis.attr("data-itemId");
                        $("#tr_" + val).removeClass("bg-light");
                    }
                });
        }
        if (all.is(":checked")) {
            $(".btnDisabledItemChecked").prop("disabled", false);
            $(".btnEnabledItemChecked").prop("disabled", false);
            $(".btnDeleteItemChecked").prop("disabled", false);

        } else {
            $(".btnDisabledItemChecked").prop("disabled", true);
            $(".btnEnabledItemChecked").prop("disabled", true);
            $(".btnDeleteItemChecked").prop("disabled", true);

        }
       

    });
});

$("#btnDeleteConfirm").on("click", function () {
    Swal.fire({
        title: "DELETE",
        html: "Are you sure you want to delete this department?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#1c3d77",
        confirmButtonText: "Yes, I am sure",
        cancelButtonText: "No, Cancel it",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            $("#btnSubmitDeleteItem").click();
        }
    });
});
$("#btnSubmitSelectedDeptDisable").on("click", function () {
    var action = "Disable";
    ActionSelecteItems(action);
});

$("#btnSubmitSelectedDeptEnable").on("click", function () {
    var action = "Enable";
    ActionSelecteItems(action);
});
$("#btnSubmitSelectedDeptDelete").on("click", function () {
    var action = "Delete";
    ActionSelecteItems(action);
    
});
function ActionSelecteItems(action) {
    var deptIds = [];
    var Item_checkbox = $("[data-checkboxes=delgroup]:not([data-checkbox-role=dad]):checked");
    for (var i = 0; i < Item_checkbox.length; i++) {
        deptIds.push(Item_checkbox[i].value)
    }
    if (deptIds.length != 0) {
        commaSeperatedDeptIds = deptIds.toString();
        $.ajax({
            url: "/Department/ActionSelectedItems",
            type: "Post",
            data: { DeptIds: commaSeperatedDeptIds, Action: action },
            success: function (response) {
                location.reload();
            }
        });
    }
}

$("#DepartmentNameInput").on("change", function () {
    var val = $(this).val();
    var filteredVal = val
        .replace("of", "")
        .replace("the", "")
        .replace("The", "")
        .replace("THE", "")
        .replace("and", "")
        .replace("And", "")
        .replace("AND", "");
    var sliced = [];
    sliced = filteredVal.split(" ");
    var SlicedVal = "";
    for (var i = 0; i < sliced.length; i++) {
        SlicedVal += sliced[i].slice(0, 1);
    }
    $("#DisplayDepartmentName").val(SlicedVal);
});


$("#DepartmentName").on("change", function () {
    var val = $(this).val();
    var filteredVal = val
        .replace("of", "")
        .replace("the", "")
        .replace("The", "")
        .replace("THE", "")
        .replace("and", "")
        .replace("And", "")
        .replace("AND", "");
    var sliced = [];
    sliced = filteredVal.split(" ");
    var SlicedVal = "";
    for (var i = 0; i < sliced.length; i++) {
        SlicedVal += sliced[i].slice(0, 1);
    }
    $("#DepartmentNameDisplay").val(SlicedVal);
});
$(".btnEditDept").on("click", function () {
    var DeptId = $(this).attr("data-deptId");
    var DeptName = $(this).attr("data-deptName");
    var DeptDisplay = $(this).attr("data-deptDisplay");
    var DeptCluster = $(this).attr("data-deptCluster");

    $("#deptId").val(DeptId);
    $("#DepartmentName").val(DeptName);
    $("#DepartmentNameDisplay").val(DeptDisplay);
    $("#DepartmentCluster").val(DeptCluster);
    $("#modal-edit-dept").modal("show");
});
$(".btnEditDeptCluster").on("click", function () {
    var DeptClusterId = $(this).attr("data-deptClusterId");
    var DeptClusterName = $(this).attr("data-deptClusterName");

    $("#ClusterId").val(DeptClusterId);
    $("#DepartmentClusterName").val(DeptClusterName);
    $("#modal-edit-deptCluster").modal("show");
});
$("#monthly1").on("change", function () {
    var val = $(this).val();
    $("#monthly2").removeAttr("Disabled", "Disabled");
    $("#monthly2").attr("min", val);
});

$("#btnUserAdd").on("click", function () {
    var btnsubmitUser = $("#submitUserBtn");
    Swal.fire({
        title: "SAVE",
        html: "Are you sure that all entered information are correct?",
        icon: "info",
        showCancelButton: true,
        confirmButtonColor: "#1c3d77",
        confirmButtonText: "Yes, I am sure",
        cancelButtonText: "No, Cancel it",
        closeOnConfirm: false,
        closeOnCancel: true
    }).then((result) => {
        if (result.isConfirmed) {
            btnsubmitUser.click();
        }
    });
});

const removeVowels = function (string) {
    let vowels = ["a", "e", "i", "o", "u", "A", "E", "I", "O", "U"];
    return [...string].filter((c) => !vowels.includes(c)).join("");
};


$("#ItemNameEdit").on("input", function () {
    var val = $(this).val();
    var WithoutVowels = removeVowels(val);
    $("#CodeNameEdit").val(WithoutVowels);
});
$("#ItemType").attr("Disabled", "Disabled");
// Supply Type
$("#SupplyType").on("change", function () {
    var val = $("#SupplyType option:selected").val();
    var itemTypeSel = $("#ItemType");

    if (val != "") {
        //Ajax Change Value of select option itemtype
        itemTypeSel.removeAttr("Disabled", "Disabled");
        $.ajax({
            url: "/Item/GetSupplyId",
            method: "Post",
            data: { supplyId: val },
            success: function (result) {
                itemTypeSel.empty(); //clear the options:
                var s = "";
                s += '<option value="">Select Item Type</option>';
                for (var i = 0; i < result.length; i++) {
                    s += '<option value="' + result[i].id + '">' + result[i].itemType + '</option>';
                }
                s += '<option value="others">Others</option>';
                itemTypeSel.html(s);
            }
        });
        $("#OtherItemType").attr("Disabled", "Disabled");
        $("#OtherItemType").removeAttr("required", "required");
    }
    else {
        $("#OtherItemType").attr("Disabled", "Disabled");
        $("#OtherItemType").removeAttr("required", "required");
        itemTypeSel.val("");
        itemTypeSel.attr("Disabled", "Disabled");
    }
});

// Item Type
$("#ItemType").on("change", function () {
    var val = $("#ItemType option:selected").val();
    $("#ItemName").val(val);
    if (val == "others") {
        $("#OtherItemType").removeAttr("Disabled", "Disabled");
        $("#OtherItemType").attr("required", "required");
    }
    else {
        $("#OtherItemType").attr("Disabled", "Disabled");
        $("#OtherItemType").removeAttr("required", "required");
    }
});

$("#btnReload").on("click", function () {
    window.location.reload();
});

$(".btnViewItem").on("click", function () {
    var Id = $(this).attr("data-itemId");
    var itemCode = $(this).attr("data-ItemCode");
    var itemName = $(this).attr("data-ItemName");
    var itemUnit = $(this).attr("data-UnitId");
    var itemType = $(this).attr("data-TypeId");
    var itemQuantity = $(this).attr("data-Quantity");
    var itemSupplyId = $(this).attr("data-supplyId");

    $("#ItemIdEdit").val(Id);
    $("#CodeNameEdit").val(itemCode);
    $("#ItemNameEdit").val(itemType);
    $("#SupplyTypeEdit").val(itemSupplyId);
    $("#QuantityEdit").val(itemQuantity);
    $("#UnitEdit").val(itemUnit);

    $(".checkboxValueEdit").attr('checked', false);
    $(".editValueof").val("");
    $(".inputEditSpecs").val("");
    $(".inputEditSpecs").removeAttr("Required", "Required");
    $(".inputEditSpecs").attr("Disabled", "Disabled");
    InMainLoader();
    var itemTypeSel = $("#ItemTypeEdit");
    $.ajax({
        url: "/Item/GetSupplyId",
        method: "Post",
        data: { supplyId: itemSupplyId },
        success: function (result) {
            itemTypeSel.empty(); //clear the options:
            var s = "";
            s += '<option value="">Select Item Type</option>';
            for (var i = 0; i < result.length; i++) {
                s += '<option value="' + result[i].id + '">' + result[i].itemType + '</option>';
            }
            s += '<option value="others">Others</option>';
            itemTypeSel.html(s);
            itemTypeSel.val(itemType);
        }
    });

    $.ajax({
        url: "/Item/GetSpecs",
        method: "Post",
        data: { ItemId: Id },
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("#Edit_" + result[i].itemSpecValue.itemSpecType.id).attr('checked', true);
                $("#EditInput_" + result[i].itemSpecValue.itemSpecType.itemSpecType).removeAttr("Disabled", "Disabled");
                $("#EditInput_" + result[i].itemSpecValue.itemSpecType.itemSpecType).attr("Required", "Required");
                $("#EditInput_" + result[i].itemSpecValue.itemSpecType.itemSpecType).val(result[i].itemSpecValue.itemSpecValue);
                $("#EditValueof_" + result[i].itemSpecValue.itemSpecType.itemSpecType).val(result[i].itemSpecValue.itemSpecType.id + "," + result[i].itemSpecValue.itemSpecValue)
                $(".inputEditSpecs").on("change", function () {
                    var Value = $(this).val();
                    var dataValue = $(this).attr("data-value");
                    var Id = $("#EditIDof_" + dataValue).val();
                    $("#EditValueof_" + dataValue).val(Id + "," + Value);
                });
            }

            OutMainLoader();
            $("#modal-edit-item").modal("show");
        }
    });

});



$("#SupplyTypeEdit").on("change", function () {
    var val = $("#SupplyTypeEdit option:selected").val();
    var itemTypeSel = $("#ItemTypeEdit");
    if (val == "") {
        $("#OtherItemTypeEdit").attr("Disabled", "Disabled");
        $("#OtherItemTypeEdit").removeAttr("required", "required");
        $("#ItemTypeEdit").attr("Disabled", "Disabled");
        $("#ItemTypeEdit").val("");
    }
    else {
        $("#ItemTypeEdit").removeAttr("Disabled", "Disabled");
    }
    //Ajax Change Value of select option itemtype
    
    $.ajax({
        url: "/Item/GetSupplyId",
        method: "Post",
        data: { supplyId: val },
        success: function (result) {
            itemTypeSel.empty(); //clear the options:
            var s = "";
            s += '<option value="">Select Item Type</option>';
            for (var i = 0; i < result.length; i++) {
                s += '<option value="' + result[i].id + '">' + result[i].itemType + '</option>';
            }
            s += '<option value="others">Others</option>';
            itemTypeSel.html(s);
        }
    });
});

$("#ItemTypeEdit").on("change", function () {
    var val = $("#ItemTypeEdit option:selected").val();
    if (val == "others") {
        $("#OtherItemTypeEdit").removeAttr("Disabled", "Disabled");
        $("#OtherItemTypeEdit").attr("required", "required");
    }
    else {
        $("#ItemNameEdit").val(val);
        $("#OtherItemTypeEdit").attr("Disabled", "Disabled");
        $("#OtherItemTypeEdit").removeAttr("required", "required");
    }
});


// Supply Filter
$("#SupplyFilter").on("change", function () {
    var val = $("#SupplyFilter option:selected").val();
    if (val == "others") {
        $("#ItemTypeFilter").removeAttr("Disabled", "Disabled");
    }
    else if (val != "others" && val != "") {
        $("#ItemTypeFilter").removeAttr("Disabled", "Disabled");
    }
    else if (val == "") {
        $("#ItemTypeFilter").attr("Disabled", "Disabled");
    }
    InMainLoader();
    $("form[name=ItemsListForm]").submit();
    //Ajax Change Value of select option itemtype
    //var itemTypeSel = $("#ItemTypeFilter");
    //$.ajax({
    //    url: "/Inventory/GetSupplyId",
    //    method: "Post",
    //    data: { supplyId: val },
    //    success: function (result) {
    //        itemTypeSel.empty(); //clear the options:
    //        var s = "";
    //        s += '<option value="">Select Item Type</option>';
    //        for (var i = 0; i < result.length; i++) {
    //            s += '<option value="' + result[i].id + '">' + result[i].itemType + '</option>';
    //        }
    //        itemTypeSel.html(s);
    //    }
    //});
});
$(".btndisableItem").on("click", function () {
    InMainLoader();
});
$(".btndeleteItem").on("click", function () {
    InMainLoader();
});
$(".btnenableItem").on("click", function () {
    InMainLoader();
});
$(".btnDeactivate").on("click", function () {
    var val = $(this).attr("data-itemId");
    $("#ItemIdDeactivate").val(val);
    
    $("#modal-item-deactivate").modal("show");
});
$(".btnActivate").on("click", function () {
    var val = $(this).attr("data-itemId");
    $("#ItemIdActivate").val(val);
    $("#modal-item-activate").modal("show");
});
$(".btnDeleteItem").on("click", function () {
    var val = $(this).attr("data-itemId");
    $("#ItemIdDelete").val(val);
    $("#modal-delete-item").modal("show");
});



$("#btnSubmitSelectedItemDisable").on("click", function () {
    var action = "Disable";
    var ItemIds = [];
    var Item_checkbox = $("[data-checkboxes=mygroup]:not([data-checkbox-role=dad]):checked");
    for (var i = 0; i < Item_checkbox.length; i++) {
        ItemIds.push(Item_checkbox[i].value)
    }
    if (ItemIds.length != 0) {
        InMainLoader();
        commaSeperatedItemIds = ItemIds.toString();
        $.ajax({
            url: "/Item/ActionSelectedItems",
            type: "Post",
            data: { itemIds: commaSeperatedItemIds, Action: action },
            success: function (response) {
                location.reload();
            }
        });
    }
});

$("#btnSubmitSelectedItemEnable").on("click", function () {
    var action = "Enable";
    var ItemIds = [];
    var Item_checkbox = $("[data-checkboxes=mygroup]:not([data-checkbox-role=dad]):checked");
    for (var i = 0; i < Item_checkbox.length; i++) {
        ItemIds.push(Item_checkbox[i].value)
    }
    if (ItemIds.length != 0) {
        InMainLoader();
        commaSeperatedItemIds = ItemIds.toString();
        $.ajax({
            url: "/Item/ActionSelectedItems",
            type: "Post",
            data: { itemIds: commaSeperatedItemIds, Action: action },
            success: function (response) {
                location.reload();
            }
        });
    }
});
$("#btnSubmitSelectedItemDelete").on("click", function () {
    var action = "Delete";
    var ItemIds = [];
    var Item_checkbox = $("[data-checkboxes=mygroup]:not([data-checkbox-role=dad]):checked");
    for (var i = 0; i < Item_checkbox.length; i++) {
        ItemIds.push(Item_checkbox[i].value)
    }
    if (ItemIds.length != 0) {
        InMainLoader();
        commaSeperatedItemIds = ItemIds.toString();
        $.ajax({
            url: "/Item/ActionSelectedItems",
            type: "Post",
            data: { itemIds: commaSeperatedItemIds, Action: action },
            success: function (response) {
                location.reload();
            }
        });
    }
});

$(".checkboxValue").on("click", function () {
    var thisValue = $(this).val();
    if (this.checked) {
        $("#" + thisValue).removeAttr("Disabled", "Disabled");
        $("#" + thisValue).attr("Required", "Required");
        $("#" + thisValue).on("change", function () {
            var Value = $(this).val();
            var Id = $("#IDof_" + thisValue).val();
            $("#Valueof_" + thisValue).val(Id + "," + Value);
        });
    } else if (!this.checked) {
        $("#" + thisValue).attr("Disabled", "Disabled");
        $("#" + thisValue).removeAttr("Required", "Required");
        $("#Valueof_" + thisValue).val("");
        $("#" + thisValue).val("");
    }
    
});

$(".checkboxValueEdit").on("click", function () {
    var thisValue = $(this).val();
    if (this.checked) {
        $("#EditInput_" + thisValue).removeAttr("Disabled", "Disabled");
        $("#EditInput_" + thisValue).attr("Required", "Required");
        $("#EditInput_" + thisValue).on("change", function () {
            var Value = $(this).val();
            var Id = $("#EditIDof_" + thisValue).val();
            $("#EditValueof_" + thisValue).val(Id + "," + Value);
        });
    } else if (!this.checked) {
        $("#EditInput_" + thisValue).attr("Disabled", "Disabled");
        $("#EditInput_" + thisValue).removeAttr("Required", "Required");
        $("#EditValueof_" + thisValue).val("");
        $("#EditInput_" + thisValue).val("");
    }

});

