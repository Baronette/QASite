$(() => {
    setInterval(() => {
        updateLikes()
    }, 1000);


    $("#like").on("click", function () {
        const questionId = $("#likes-count").data("question-id");
        $.post("/questions/likequestion", { questionId }, function () {
            updateLikes();
            $("#like").prop("disabled", true);
        });
    });


    function updateLikes() {
        const id = $("#likes-count").data("question-id");
        $.get("/questions/getlikes", { id }, function ({ likes }) {
            $("#likes-count").html(likes);
        });
    };
})
