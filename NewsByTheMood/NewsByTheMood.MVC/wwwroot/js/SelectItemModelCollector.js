class SelectItemModelCollector {
    #form;
    #hiddenDataContainer;
    #models = [];

    constructor(formId) {
        this.#hiddenDataContainer = document.createElement("div");
        this.#hiddenDataContainer.classList.add("hidden-data");
        this.#hiddenDataContainer.style.display = "none";

        this.#form = document.getElementById(formId);
        if (this.#form) {
            this.#form.appendChild(this.#hiddenDataContainer);
        }
    }

    setModelItems(itemsSelector, modelName) {
        this.#models.push({
            selector: itemsSelector,
            modelName: modelName
        });
    }

    collectModels() {
        if (!Array.isArray(this.#models) || this.#models.length === 0) {
            console.error("No models to iterate over.");
            return;
        }
        if (!this.#hiddenDataContainer) {
            console.error("Hidden data container is not initialized.");
            return;
        }
        if (!this.#form) {
            console.error("Form is not initialized.");
            return;
        }

        this.#hiddenDataContainer.innerHTML = "";

        for (let model of this.#models) {
            if (!model.selector || typeof model.selector !== "string") {
                console.error("Invalid selector:", model.selector);
                continue;
            }

            var items = document.querySelectorAll(model.selector);
            if (items.length === 0) {
                console.error("No items found for selector:", model.selector);
                continue;
            }

            for (let i = 0; i < items.length; i++) {
                var item = items[i];

                var input = document.createElement("input");
                input.type = "hidden";
                input.name = `${model.modelName}[${i}].Value`;
                input.value = item.value;
                this.#hiddenDataContainer.appendChild(input);

                var input = document.createElement("input");
                input.type = "hidden";
                input.name = `${model.modelName}[${i}].Text`;
                input.value = item.textContent;
                this.#hiddenDataContainer.appendChild(input);

                var input = document.createElement("input");
                input.type = "hidden";
                input.name = `${model.modelName}[${i}].Selected`;
                input.value = item.selected;
                this.#hiddenDataContainer.appendChild(input);
            }
        }
    }
}

