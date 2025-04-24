document.getElementById("addCommentForm").addEventListener("submit", async function (event) {
    const form = event.target;
    const validator = $(form).validate();

    if (!validator.form()) {
        form.reportValidity();
        return;
    }

    event.preventDefault();
    const formData = new FormData(form);
    
    try {
        const response = await fetch(form.action, {
            method: form.method,
            body: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "X-CSRF-TOKEN": formData.get("__RequestVerificationToken"),
            }
        });

        if (!response.ok) {
            const message = "Failed add comment while sending to server.";
            console.error(message + "Response code: " + response.status);
            validator.showErrors({
                Text: message
            });
            return;
        }
        else {
            window.alert("Comment added successfully");
        }
    }
    catch (error) {
        const message = "Error while creating comment.";
        console.error(message, error);
        validator.showErrors({
            Text: message + " Reload page"
        });
    }
});