async function sendAsyncRequest(link, method, redirectUrl, notifySender) {
	if (link) {
		try {
			const response = await fetch(link, {
				method: method,
				body: null,
				headers: {
					"X-Requested-With": "XMLHttpRequest"
				}
			});

			if (response.ok) {
				if (typeof redirectUrl === "string") {
					window.location.href = redirectUrl;
				}
			}
			else {
				const result = await response.text();
				const message = "Http code: " + response.status + " " + result;
				console.error(message);
				if (typeof notifySender === "function") {
					notifySender(message, 'error')
				}
			}
		}
		catch (error) {
			const message = error;
			console.error(message);
			if (typeof notifySender === "function") {
				notifySender(message, 'error')
			}
		}
	}
	else {
		const message = "Bad request. No found link to endpoint.";
		console.error(message);
		if (typeof notifySender === "function") {
			notifySender(message, 'error')
		}
	}
}