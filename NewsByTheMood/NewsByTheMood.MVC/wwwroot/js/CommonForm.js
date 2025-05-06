class CommonForm {
    #form;
    #submitButton;

    #successMessage;
    #successRedirectUrl;
    #errorMessage;

    #notifySender;

    constructor(formId, submitButtonId, successMessage, successRedirectUrl, errorMessage, notifySender) {
        this.#form = document.getElementById(formId);
        this.#submitButton = document.getElementById(submitButtonId);

        this.#successMessage = successMessage;
        this.#successRedirectUrl = successRedirectUrl;
        this.#errorMessage = errorMessage;

        this.#notifySender = notifySender;

        this.#submitButton.addEventListener("click", async (event) => {
            await this.submit(event);
        });
    }

    async submit(event) {
        event.preventDefault();

        const formValidator = $(this.#form).validate();

        if (!formValidator.form()) {
            const message = "Please fill in all required fields correctly";
            console.warn(message);
            this.#form.reportValidity();
            return;
        }

        this.#submitButton.disabled = true;

        const formData = new FormData(this.#form);

        try {
            const response = await CommonForm.send(this.#form.action, this.#form.method, formData, {});
            if (response.ok) {
                console.log(this.#successMessage);

                if (typeof this.#notifySender === "function") {
                    this.#notifySender(this.#successMessage, 'success')
                }

                if (typeof this.#successRedirectUrl === "string") {
                    window.location.href = this.#successRedirectUrl;
                }
            }
            else {
                const contentType = response.headers.get("Content-Type") || "";
                const hasBody = contentType.includes("application/json");

                if (hasBody) {
                    const result = await response.json();
                    const fullErrorMessage = this.#errorMessage + " HTTP code: " + response.status + "\n" +
                        JSON.stringify(result);

                    console.error(fullErrorMessage);

                    if (result.generalErrors && typeof this.#notifySender === "function") {
                        this.#notifySender(result.generalErrors, 'error');
                        delete result.generalErrors;
                    }

                    if (result) {
                        try {
                            formValidator.showErrors(result);
                        }
                        catch (error) {
                            console.error("Error while showing validation errors: " + error);
                            if (typeof this.#notifySender === "function") {
                                this.#notifySender(fullErrorMessage, 'error');
                            }
                        }
                    }
                }
                else {
                    const errorMessage = this.#errorMessage + " HTTP code: " + response.status;
                    console.error(errorMessage);
                    if (typeof this.#notifySender === "function") {
                        this.#notifySender(errorMessage, 'error');
                    }
                }
            }
        }
        catch (error) {
            const errorMessage = this.#errorMessage + " " + error;
            console.error(errorMessage);
            if (typeof this.#notifySender === "function") {
                this.#notifySender(errorMessage, 'error');
            }
        }
        finally {
            this.#submitButton.disabled = false;
        }
    }

    static async send(action, method, body, headers) {
        const response = await fetch(action, {
            method: method,
            body: body,
            headers: {
                "X-Requested-With": "XMLHttpRequest",
                "X-CSRF-TOKEN": body.get("__RequestVerificationToken"),
                ...headers
            }
        });

        return response;
    }
}
