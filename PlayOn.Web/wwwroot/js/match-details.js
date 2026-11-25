// Refreshes the roster every 15 seconds. Replaced by SignalR (MatchHub.RosterChanged) in the bonus phase.
(function () {
    const card = document.querySelector('[data-roster-url]');
    if (!card) return;

    const url = card.dataset.rosterUrl;
    if (!url) return;

    async function refresh() {
        const rosterEl = card.querySelector('#roster');
        if (!rosterEl) return;

        try {
            const response = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            if (!response.ok) return;

            const html = await response.text();
            const template = document.createElement('template');
            template.innerHTML = html;
            rosterEl.replaceWith(template.content);
        } catch {
            // Ignore transient network errors during periodic polling
        }
    }

    setInterval(refresh, 15000);
})();