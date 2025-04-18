class ModelProperty {
    #propertyName;
    #isArray;

    #selector;

    #valueHandler
    #valueSelector;

    constructor(propertyName, selector, isArray = false, valueHandler = "getValueFromAttribute", valueSelector = null) {
        this.#propertyName = propertyName;
        this.#selector = selector;
        this.#isArray = isArray;
        this.#valueHandler = valueHandler;
        this.#valueSelector = valueSelector;
    }

    getValues() {
        if (typeof (this.#selector) === typeof (ModelProperty)) {
            return this.#getValuesFromModelProperty();
        }
        else if (typeof (this.#selector) === typeof (String)) {
            this.#getValuesFromCssSelector();
        }
        else {
            throw new Error("Unknown type of selector");
        }
    }

    #getValuesFromCssSelector() {
        if (this.#isArray) {
            const elements = document.querySelectorAll(this.#selector);

            if (elements.length !== 0) {
                const items = [];
                let name;
                let value;
                for (const i = 0; i < elements.length; i++) {
                    name = `${this.#propertyName}[${i}]`;
                    switch (this.#valueHandler) {
                        case "getValueFromAttribute":
                            value = this.#getValueFromAttribute(elements[i], this.#valueSelector);
                            break;

                        case "getValueFromInput":
                            value = this.#getValueFromInput(elements[i]);
                            break;

                        case "getValueFromTextContent":
                            value = this.#getValueFromTextContent(elements[i]);
                            break;
                    }

                    items.push(
                        {
                            name: name,
                            value: value
                        });
                }

                return items;
            }
            else {
                throw new Error("No items found for selector:", this.#selector); 
            }
        }
        else {
            const element = document.querySelector(this.#selector);
            if (element) {
                return {
                    name: this.#propertyName,
                    value: this.#valueHandler(element)
                };
            }
            else {
                throw new Error("No item found for selector:", this.#selector);
            }
        }
    }

    #getValuesFromModelProperty() {
        if (this.#isArray) {
            const elements = this.#selector.getValues();

            if (elements.length !== 0) {
                const items = [];
                let name;
                let value;
                for (const i = 0; i < elements.length; i++) {
                    name = `${this.#propertyName}[${i}]`;
                    value = elements[i];
                }

                items.push(
                    {
                        name: name,
                        value: value
                    });

                return items;
            }
            else {
                throw new Error("No items found for selector:", this.#selector);
            }
        }
        else {
            const element = this.#selector.getValues();
            if (element) {
                return {
                    name: this.#propertyName,
                    value: element
                };
            }
            else {
                throw new Error("No item found for selector:", this.#selector);
            }
        }
    }

    #getValueFromAttribute(element, attributeName) {
        return element.getAttribute(attributeName);
    }

    #getValueFromInput(element) {
        return element.value;
    }

    #getValueFromTextContent(element) {
        return element.textContent;
    }
}

function parseModelProperty(modelProperty, formId) {
    const form = document.getElementById(formId);
    const hiddenDataContainer;
    if (form) {
        hiddenDataContainer = document.createElement("div");
        hiddenDataContainer.classList.add("hidden-data");
        hiddenDataContainer.style.display = "none";
        form.appendChild(hiddenDataContainer);
    }
    else {
        throw new Error("Form with ID", formId, "not found.");
    }

    for (const prop in modelProperty.getValues()) {

    }
}