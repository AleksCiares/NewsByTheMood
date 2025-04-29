document.addEventListener("DOMContentLoaded", async function () {
    document.getElementById("userForm").addEventListener("submit", async function (event) {
        const form = event.target;
        const validator = $(form).validate();
        const generalErrorContainer = document.getElementById("general-error");

        if (!validator.form()) {
            const message = "Please fill in all required fields.";
            console.warn(message);
            form.reportValidity();
            return;
        }
        else {
            generalErrorContainer.style.display = "none";
            generalErrorContainer.textContent = "";
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
                const contentType = response.headers.get("Content-Type") || "";
                const hasBody = contentType.includes("application/json");

                if (hasBody) {
                    const result = await response.json();
                    const message = "Failed save comment. Response code: " + response.status + " " +
                        JSON.stringify(result);
                    console.error(message);

                    if (result.error) {
                        generalErrorContainer.textContent = result.error;
                        generalErrorContainer.style.display = "block";
                        delete result.error;
                    }
                    if (!result) {
                        generalErrorContainer.textContent = message;
                        generalErrorContainer.style.display = "block";
                    }
                    else {
                        validator.showErrors(result);
                    }
                }
                else {
                    const message = "Failed save comment. Response code: " + response.status;
                    generalErrorContainer.textContent = message;
                    generalErrorContainer.style.display = "block";
                }
            }
        }
        catch (error) {
            const message = "Error while saving comment. " + error;
            console.error(message);
            generalErrorContainer.textContent = message;
            generalErrorContainer.style.display = "block";
        }
    });
});