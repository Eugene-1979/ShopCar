// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {



    $("table#pages tbody").sortable({
        items: "tr:not(.home)",
        placeholder: "ui-state-highlight",
        update: function () {
            var ids = $("table#pages tbody").sortable("serialize");
          
           /*  var url = "";
            $.post(url, id, function (data) { });*/
        }
    });


    $("#SelectCategory").on("change", function () {
        var url = $(this).val();
        if (url) {
            var tt = "catId="+url;
          
            /*   $.get(tt, function () { })*/
          

            window.location.search = tt;
        }
        return false;
    });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $("img#imgpreview")
                    .attr("src", e.target.result)
                    .width(370).
                    height(200);
               
            }

            reader.readAsDataURL(input.files[0]);

        }


    }

    $("#imageUpload").change(function () {
   
        readURL(this);
    })


/*
    Dropzone.options.dropzoneForm = {
        acceptedFiles: "images/*",
        init: function () {
            this.on("complete",
                function (file) {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        location.reload();
                    }
                });
            this.on("sending",
                function (file, xhr, formData) {
                    formData.append("id",  @Model.Id);
        });
        }
};
*/



});