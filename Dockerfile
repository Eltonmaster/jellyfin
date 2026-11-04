# Builds the patched Jellyfin server and swaps it into the official image,
# which already provides the web client, jellyfin-ffmpeg and GPU drivers.
# Build context: a jellyfin/jellyfin checkout with the patches applied.

ARG BASE_IMAGE=jellyfin/jellyfin:latest
ARG DOTNET_VERSION=10.0

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS server
ARG TARGETARCH
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
WORKDIR /src
COPY . .
RUN case "${TARGETARCH}" in \
        amd64) arch=x64 ;; \
        arm64) arch=arm64 ;; \
        *) echo "Unsupported architecture: ${TARGETARCH}" >&2; exit 1 ;; \
    esac \
 && dotnet publish Jellyfin.Server --arch "${arch}" --configuration Release \
    --output /server --self-contained \
    -p:DebugSymbols=false -p:DebugType=none

FROM ${BASE_IMAGE}
RUN find /jellyfin -mindepth 1 -maxdepth 1 ! -name jellyfin-web -exec rm -rf {} +
COPY --from=server /server /jellyfin
ARG PATCHES=""
ARG UPSTREAM_REF=""
LABEL org.opencontainers.image.title="Jellyfin (patched)" \
      org.opencontainers.image.description="Jellyfin with patches: ${PATCHES}" \
      org.opencontainers.image.revision="${UPSTREAM_REF}"
