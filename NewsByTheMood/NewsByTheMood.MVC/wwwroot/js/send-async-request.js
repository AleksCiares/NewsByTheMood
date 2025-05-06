async function sendAsyncRequest(link, method, redirectUrl, notifySender, body, headers) {
	if (link) {
		try {
			const response = await fetch(link, {
				method: method,
				body: body,
				headers: {
					"X-Requested-With": "XMLHttpRequest",
					...headers
				},
			});

			if (response.ok) {
				if (typeof redirectUrl === "string") {	
					window.location.href = redirectUrl;
				}

                return true;
			}
			else {
				const result = await response.text();
				const message = "Http code: " + response.status + " " + result;
				console.error(message);
				if (typeof notifySender === "function") {
					notifySender(message, 'error')
				}

				return false;
			}
		}
		catch (error) {
			const message = error;
			console.error(message);
			if (typeof notifySender === "function") {
				notifySender(message, 'error')
			}

            return false;
		}
	}
	else {
		const message = "Bad request. No found link to endpoint.";
		console.error(message);
		if (typeof notifySender === "function") {
			notifySender(message, 'error')
		}

        return false;
	}
}