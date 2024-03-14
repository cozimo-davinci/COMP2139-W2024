function loadComments(projectID) {
    $ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectID=' + projectID,
        method: 'GET',
        success: function (data) {
            var commentHtml = '';
            for (var i = 0; data.length; i++) {
                commentHtml += '<div class="comment"">';
                commentHtml += '<p>' + data[i].content + '</p>';
                commentHtml += '<span>Posted on ' + new Date(data[i].datePosted).toLocaleDateString + '</span>';
                commentHtml += '</div>';
            }
            $('#commentList').html(commentHtml);
        }
    });
}

$(document).ready(function () {

    var projectID = $('#projectComments input[name="projectID"]').val();

    loadComment(projectID);

    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            projectID: projectID,
            Content: $('#projectComments input[name="Content"]').val()
        };
         

    });

    $ajax({
        url: '/ProjectManagement/ProjectComment/AddComment',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                $('#projectComments input[name="Content"]').val(''); //clear message area 
                loadComments(projectID);
            } else {
                alert(response.message);
            }
        },

        error: function (xhr, status, error) {
            alert("Error " + error);
        }
    });
});