#
# Authors:
# Copyright 2012-2019 Peter van der Does. All rights reserved.
#
# Original Author:
# Copyright 2010 Vincent Driessen. All rights reserved.
#
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are met:
#
# 1. Redistributions of source code must retain the above copyright notice, this
#    list of conditions and the following disclaimer.
# 2. Redistributions in binary form must reproduce the above copyright notice,
#    this list of conditions and the following disclaimer in the documentation
#    and/or other materials provided with the distribution.
#
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
# ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
# WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
# DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
# ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
# (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
# LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
# ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
# (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
# SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#

prefix=/usr/local

datarootdir=$(prefix)/share
docdir=$(datarootdir)/doc/gitflow
# files that need mode 755
EXEC_FILES=git-flow

# files that need mode 644
SCRIPT_FILES =git-flow-init
SCRIPT_FILES+=git-flow-feature
SCRIPT_FILES+=git-flow-bugfix
SCRIPT_FILES+=git-flow-hotfix
SCRIPT_FILES+=git-flow-release
SCRIPT_FILES+=git-flow-support
SCRIPT_FILES+=git-flow-version
SCRIPT_FILES+=git-flow-log
SCRIPT_FILES+=git-flow-config
SCRIPT_FILES+=gitflow-common
SCRIPT_FILES+=gitflow-shFlags

# Hook files
HOOK_FILES=$(wildcard hooks/*)

all:
	@echo "usage: make install"
	@echo "       make uninstall"

install:
	install -d -m 0755 $(prefix)/bin
	install -d -m 0755 $(docdir)/hooks
	install -m 0755 $(EXEC_FILES) $(prefix)/bin
	install -m 0644 $(SCRIPT_FILES) $(prefix)/bin
	install -m 0644 $(HOOK_FILES) $(docdir)/hooks

uninstall:
	test -d $(prefix)/bin && \
	cd $(prefix)/bin && \
	rm -f $(EXEC_FILES) $(SCRIPT_FILES)
	test -d $(docdir) && \
	rm -rf $(docdir)
