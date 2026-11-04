# Patched Jellyfin images

This branch only holds the build setup. The Jellyfin source comes straight from
[jellyfin/jellyfin](https://github.com/jellyfin/jellyfin) on every build, with the
commits of the patch branches listed in `PATCH_BRANCHES` applied on top.

Current patches:

- [`fix/strm-download`](../../tree/fix/strm-download): downloading a `.strm` item returns the media it points to

## Images

| Image | Based on |
|---|---|
| `ghcr.io/eltonmaster/jellyfin:stable` | latest Jellyfin release (`release-*` branch, official `jellyfin/jellyfin:<version>` image) |
| `ghcr.io/eltonmaster/jellyfin:unstable` | Jellyfin `master` (official `jellyfin/jellyfin:unstable` image) |

Every build is also tagged `<channel>-<date>-<upstream commit>` for rollbacks.
Both `linux/amd64` and `linux/arm64` are built.

The images are drop-in replacements for the official ones: same paths, volumes, ports and environment.

## Updating

Actions → **Build patched image** → **Run workflow**. This picks up the latest Jellyfin
changes and rebuilds. If a patch no longer applies, the run fails and says which branch to rebase.

## Adding a patch

Create a branch from jellyfin `master`, push it here and add its name to `PATCH_BRANCHES`
in [`.github/workflows/build-image.yml`](.github/workflows/build-image.yml).
