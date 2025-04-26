document.addEventListener("DOMContentLoaded", async function () {
    document.getElementById("userForm").addEventListener("submit", async function (event) {
        const form = event.target;
        const validator = $(form).validate();

        if (!validator.form()) {
            const message = "Please fill in all required fields.";
            console.log(message);
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

            if (response.ok) {
                const data = await response.text();
                document.getElementById("asyncLoadItemsContainer").insertAdjacentHTML("afterbegin", data);

                form.reset();
                validator.resetForm();

                // restore textarea height
                const textArea = form.querySelector("[dynamic-textarea]");
                if (textArea) {
                    textArea.style.height = "";
                }
            }
            else {
                const message = "Failed saving comment.";
                console.error(message + " Response code: " + response.status);
                validator.showErrors({
                    Text: message
                });
            }
        }
        catch (error) {
            const message = "Error while creating comment.";
            console.error(message, error);
            validator.showErrors({
                Text: message + " Reload page."
            });
        }
    });
});