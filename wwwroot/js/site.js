
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
    $("#ItemSel").select2();

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
});

$("[data-checkboxes]").each(function () {
    var me = $(this),
        group = me.data("checkboxes"),
        role = me.data("checkbox-role");
    mecheck(me, group, role);
});
function mecheck(me, group, role) {
    me.change(function () {
        var Container = $("#ItemsSelContainer");
        var itemChecked = $("[data-checkboxes]:checked");
        var array = [];
        console.log(array.toString());
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

                me.prop("checked", true);
            } else {
                me.prop("checked", false);
            }
            if (dad.is(":checked")) {

                all.prop("checked", true);
                all.each(function () {
                    var val = $(this).attr("data-itemId");
                    $("#tr_" + val).addClass("bg-light");
                });
            } else {

                all.prop("checked", false);
                all.each(function () {
                    var val = $(this).attr("data-itemId");

                    $("#tr_" + val).removeClass("bg-light");
                });
            }
        } else {
            if (checked_length >= total) {

                dad.prop("checked", true);
            } else {

                dad.prop("checked", false);
            }

            all.each(function () {
                var ethis = $(this);
                if (ethis.is(":checked")) {
                    var val = ethis.attr("data-itemId");
                    $("#tr_" + val).addClass("bg-light");
                } else {
                    var val = ethis.attr("data-itemId");
                    $("#tr_" + val).removeClass("bg-light");
                }
            });
        }
        if (all.is(":checked")) {

        } else {
        }


    });
}
const removeVowels = function (string) {
    let vowels = ["a", "e", "i", "o", "u", "A", "E", "I", "O", "U"];
    return [...string].filter((c) => !vowels.includes(c)).join("");
};

$("#btnReload").on("click", function () {
    window.location.reload();
});

