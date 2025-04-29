document.addEventListener("DOMContentLoaded", async function () {
    const commonForm_formElement = document.getElementById(commonForm_formId);
    const commonForm_submitButton = commonForm_formElement.querySelector("button[type='submit']");

    commonForm_formElement.addEventListener("submit", async function (event) {
        const form = event.target;
        const validator = $(form).validate();

        if (!validator.form()) {
            const message = "Please fill in all required fields correctly";
            console.warn(message);
            form.reportValidity();

            return;
        }

        if (commonForm_submitButton) {
            commonForm_submitButton.disabled = true;
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
                console.log(commonForm_successMessage);

                if (typeof showGeneralNotifyMessage === "function") {
                    showGeneralNotifyMessage(commonForm_successMessage, 'success')
                }

                if (typeof commonForm_successRedirectUrl !== "undefined") {
                    window.location.href = commonForm_successRedirectUrl;
                }
            }
            else {
                const contentType = response.headers.get("Content-Type") || "";
                const hasBody = contentType.includes("application/json");

                if (hasBody) {
                    const result = await response.json();
                    const fullErrorMessage = commonForm_errorMessage + " HTTP code: " + response.status + "\n" +
                        JSON.stringify(result);

                    console.error(fullErrorMessage);

                    if (result.generalErrors && typeof showGeneralNotifyMessage === "function") {
                        showGeneralNotifyMessage(result.generalErrors, 'error');
                        delete result.generalErrors;
                    }

                    if (result) {
                        try {
                            validator.showErrors(result);
                        }
                        catch (error) {
                            console.error("Error while showing validation errors: " + error);
                            if (typeof showGeneralNotifyMessage === "function") {
                                showGeneralNotifyMessage(fullErrorMessage, 'error');
                            }
                        }
                    }
                }
                else {
                    const errorMessage = commonForm_errorMessage + " HTTP code: " + response.status;
                    console.error(errorMessage);
                    if (typeof showGeneralNotifyMessage === "function") {
                        showGeneralNotifyMessage(errorMessage, 'error');
                    }
                }
            }
        }
        catch (error) {
            const errorMessage = commonForm_errorMessage + " " + error;
            console.error(errorMessage);
            if (typeof showGeneralNotifyMessage === "function") {
                showGeneralNotifyMessage(errorMessage, 'error');
            }
        }
        finally {
            if (commonForm_submitButton) {
                commonForm_submitButton.disabled = false;
            }
        }
    });
});