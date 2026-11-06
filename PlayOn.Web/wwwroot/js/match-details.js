// Refreshes the roster every 15 seconds. Replaced by SignalR (MatchHub.RosterChanged) in the bonus phase.
(function () {
    const card = document.querySelector('[data-roster-url]');
    if (!card) return;

    async function refresh() {
        const response = await fetch(card.dataset.rosterUrl);   // returns the rendered _RosterList partial
        if (!response.ok) return;
        const html = await response.text();
        card.querySelector('#roster')?.replaceWith(
            Object.assign(document.createElement('template'), { innerHTML: html }).content);
    }

    setInterval(refresh, 15000);
})();
